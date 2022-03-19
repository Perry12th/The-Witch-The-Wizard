using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueFrogScript  : MonoBehaviour, IDamagable
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private Collider bodyCollider;
    [SerializeField]
    private Collider spikeCollider;
    [SerializeField]
    private Renderer bodyRenderer;
    [SerializeField]
    private Renderer spikeRenderer;
    [SerializeField]
    private Color deathColor;
    private FrogStates frogState = FrogStates.IDLE;

    private enum FrogStates
    { 
        IDLE,
        JUMPING,
        HURTING,
        DYING,
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && frogState != FrogStates.DYING)
        {

            if (!spikeCollider.enabled && collision.GetContact(0).point.y > transform.position.y)
            {
                collision.gameObject.GetComponent<WitcherScript>().GetRigidBody().velocity += new Vector3(0, 20.0f, 0);
                ApplyDamage();
            }
            else
            {
                collision.gameObject.GetComponent<WitcherScript>().ApplyDamage();
            }
            
        }
        
    }

    public void ApplyDamage(int damageTaken = 1)
    {
        if (spikeCollider.enabled)
        {
            spikeCollider.enabled = false;
            spikeRenderer.enabled = false;
            animator.SetTrigger("Hurt");
            frogState = FrogStates.HURTING;
        }
        else
        {
            animator.SetTrigger("Death");
            frogState = FrogStates.DYING;
        }
    }
    public void TurnToBlack()
    {
        StartCoroutine(TurningToBlack());
    }

    IEnumerator TurningToBlack()
    {
        Color bodyStartColor = bodyRenderer.material.color;
        float animTime = (animator.GetCurrentAnimatorStateInfo(0).length * 0.5f);
        float timer = 0;

        while (timer < animTime)
        {
            timer += Time.deltaTime;
            bodyRenderer.material.SetColor("_Color", Color.Lerp(bodyStartColor, deathColor, timer / animTime));

            yield return new WaitForEndOfFrame();
        }
    }

    public void Recover()
    {
        frogState = FrogStates.IDLE;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
