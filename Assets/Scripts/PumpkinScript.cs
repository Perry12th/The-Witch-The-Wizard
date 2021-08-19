using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinScript : MonoBehaviour
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
    private PumpkinStates pumpkinState = PumpkinStates.MOVING;
    public bool playerWithinRange;

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
        if (playerWithinRange && !animator.GetCurrentAnimatorStateInfo(0).IsName("Pumpkin_Attack"))
        {
            AttackPlayer();
        }
        if (pumpkinState == PumpkinStates.MOVING)
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
        if (collision.gameObject.CompareTag("FireBall"))
        {
            Destroy(collision.gameObject);
            life--;

            if (life == 0)
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

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Path") && pumpkinState == PumpkinStates.MOVING)
        {
            pumpkinState = PumpkinStates.FLIPPING;
            animator.SetTrigger("Flip");
        }
    }

    public void FinishFlip()
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, -transform.localEulerAngles.y, transform.localEulerAngles.z);
        ReturnToMoving();
    }

    public void ReturnToMoving()
    {
        if (!playerWithinRange)
        animator.SetTrigger("Move");
        pumpkinState = PumpkinStates.MOVING;
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
}
