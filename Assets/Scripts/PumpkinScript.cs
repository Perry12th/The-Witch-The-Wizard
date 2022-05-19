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

    private PumpkinStates pumpkinState = PumpkinStates.MOVING;
    private Vector3 targetRotation;
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
        if (playerSpotter.playerWithinRange && life > 0 && pumpkinState == PumpkinStates.MOVING)
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
                collision.gameObject.GetComponent<WitcherScript>().ApplyDamage();
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
            targetRotation = transform.rotation.eulerAngles;
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
        //transform.Rotate(Vector2.up, 180.0f);
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
        if (mustPerformFlip && pumpkinState != PumpkinStates.FLIPPING)
        {
            pumpkinState = PumpkinStates.FLIPPING;
            animator.SetTrigger("Flip");
        }
        else 
        {
            pumpkinState = PumpkinStates.MOVING;
            animator.SetTrigger("Move");    
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
        float timer = 0;
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
    private IEnumerator TurningToBlack()
    {
        Color headStartColor = headRenderer.material.color;
        Color leafStartColor = leafRenderer.material.color;
        Color candleStartColor = candleRenderer.material.color;
        float animTime = (animator.GetCurrentAnimatorStateInfo(0).length * 0.5f);
        float timer = 0;

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

    public void StartTurning()
    {
        if (mustPerformFlip)
        {
            StopCoroutine("StartTurning");
            animator.applyRootMotion = false;
            if (targetRotation != Vector3.zero)
            {
                targetRotation = new Vector3(targetRotation.x, targetRotation.y > 360.0f ? targetRotation.y - 180.0f : targetRotation.y + 180.0f, targetRotation.z);
            }
            StartCoroutine(Turning());
        }
        
    }

    private IEnumerator Turning()
    {
        Vector3 startingRotation = transform.rotation.eulerAngles;
        float totalTimeLeft = animator.GetCurrentAnimatorStateInfo(0).length * (1 -(animator.GetCurrentAnimatorStateInfo(0).normalizedTime - (int)animator.GetCurrentAnimatorStateInfo(0).normalizedTime));
        float timer = 0;
        while (timer < totalTimeLeft && pumpkinState == PumpkinStates.HURTING)
        {
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(startingRotation), Quaternion.Euler(targetRotation), timer / totalTimeLeft);
            timer += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("TurningDone");
        transform.rotation = Quaternion.Euler(targetRotation);
        targetRotation = Vector3.zero;
        mustPerformFlip = false;
        animator.applyRootMotion = true;
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
    public void SpawnSmokePoof()
    {
        EffectsManager.instance.SpawnSmokePoof(transform);
    }

}
