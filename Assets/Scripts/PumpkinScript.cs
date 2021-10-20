using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinScript : MonoBehaviour, IDamagable
{
    [SerializeField]
    private GameObject path;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private int life;
    [SerializeField]
    private Collider attackCollider;
    [SerializeField]
    private float speed;
    [SerializeField]
    private Renderer headRenderer;
    [SerializeField]
    private Renderer leafRenderer;
    [SerializeField]
    private Renderer candleRenderer;
    [SerializeField]
    private GameObject candleLight;
    [SerializeField]
    private Color deathColor;
    [SerializeField]
    private float flashTimeRate = 0.2f;
    [SerializeField]
    private PlayerSpotter playerSpotter;
    private float timer = 0;
    private PumpkinStates pumpkinState = PumpkinStates.MOVING;
    private bool outsidePath = false;
    private bool mustPerformFlip = false;
    private enum PumpkinStates
    {
        IDLE,
        MOVING,
        ATTACKING,
        FLIPPING,
        HURTING,
        DYING,
    }


    private void Start()
    {
        path.transform.parent = null;
    }

    private void Update()
    {
        if (playerSpotter.playerWithinRange && life > 0 && pumpkinState != PumpkinStates.ATTACKING)
        {
            AttackPlayer();
        }
        else if (pumpkinState == PumpkinStates.MOVING)
        {
            transform.Translate(transform.InverseTransformDirection(transform.forward) * speed * Time.deltaTime);
        }
    }

    private void AttackPlayer()
    {
        pumpkinState = PumpkinStates.ATTACKING;
        animator.SetTrigger("Attack");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (life > 0)
            {
                collision.gameObject.GetComponent<WitcherScript>().PlayerDeath();
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Path") && life > 0)
        {
            outsidePath = true;
            mustPerformFlip = true;
            pumpkinState = PumpkinStates.FLIPPING;
            animator.SetTrigger("Flip");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Path") && life > 0)
        {
            outsidePath = false;
        }
    }

    public void FinishFlip()
    {
        //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, -transform.localEulerAngles.y, transform.localEulerAngles.z);
        transform.Rotate(Vector2.up, 180.0f);
        //animator.enabled = false;
        mustPerformFlip = false;
        if (life > 0)
        {
            ReturnToMoving();
        }
    }

    public void ReturnToMoving()
    {
        //StopAllCoroutines();
        if (mustPerformFlip)
        {
            pumpkinState = PumpkinStates.FLIPPING;
            animator.SetTrigger("Flip");
            return;
        }
        else 
        {
            animator.SetTrigger("Move");
            pumpkinState = PumpkinStates.MOVING;
        }
          
    }


    public void PerformAttack()
    {
        attackCollider.enabled = true;
    }

    public void DisableCollider()
    {
        attackCollider.enabled = false;
    }


    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void TurnToBlack()
    {
        StartCoroutine(TurningToBlack());
    }

    public void StartFlashing()
    {
        StartCoroutine(Flashing());
    }

    private IEnumerator Flashing()
    {
        float flashTime = animator.GetCurrentAnimatorStateInfo(0).length;
        bool flashing = false;
        timer = 0;
        float flashTimer = 0;

        while (timer < flashTime)
        {
            timer += Time.deltaTime;
            flashTimer += Time.deltaTime;

            if (flashTimer >= flashTimeRate)
            {
                flashTimer = 0;
                flashing = !flashing;

                // Flashing = true means the renderers are disabled
                headRenderer.enabled = !flashing;
                leafRenderer.enabled = !flashing;
                candleRenderer.enabled = !flashing;
                candleLight.SetActive(!flashing);
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public void StopFlashing()
    {
        StopAllCoroutines();
        headRenderer.enabled = true;
        leafRenderer.enabled = true;
        candleRenderer.enabled = true;
        candleLight.SetActive(true);
    }

    IEnumerator TurningToBlack()
    {
        Color headStartColor = headRenderer.material.color;
        Color leafStartColor = leafRenderer.material.color;
        Color candleStartColor = candleRenderer.material.color;
        float animTime = (animator.GetCurrentAnimatorStateInfo(0).length * 0.5f);
        timer = 0;

        Debug.Log("Turn To Black");        
        while (timer < animTime)
        {
            timer += Time.deltaTime;
            headRenderer.material.SetColor("_Color", Color.Lerp(headStartColor, deathColor, timer / animTime));
            leafRenderer.material.SetColor("_Color", Color.Lerp(leafStartColor, deathColor, timer / animTime));
            candleRenderer.material.SetColor("_Color", Color.Lerp(candleStartColor, deathColor, timer / animTime));

            yield return new WaitForEndOfFrame();
        }
    }

    public void ApplyDamage(int damageTaken = 1)
    {
        life -= damageTaken;

        if (life <= 0 && pumpkinState != PumpkinStates.DYING)
        {
            pumpkinState = PumpkinStates.DYING;
            animator.SetTrigger("Death");
        }
        else if (life > 0)
        {
            animator.SetTrigger("Hurt");
            pumpkinState = PumpkinStates.HURTING;
        }
    }
}
