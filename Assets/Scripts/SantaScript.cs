using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantaScript : MonoBehaviour
{
    [SerializeField]
    GameObject snowballPrefab;
    [SerializeField]
    Transform projPosition;
    [SerializeField]
    GameObject portalPointA;
    [SerializeField]
    GameObject portalPointB;
    [SerializeField]
    Animator animator;
    [SerializeField]
    Rigidbody rigidbody;

    public float runSpeed;
    public bool isFlipping = false;
    public bool isSliding = false;
    public bool isAttacking = false;
    public bool canAttack = false;
    public bool isPlayerInRange = false;
    public bool isStunned = false;
    public int life;
    public float attackCooldownTime = 1.0f;

    private void Start()
    {
        life = 4;
    }

    private void Update()
    {
        if (!isStunned)
        {
            if (isPlayerInRange && !isAttacking && canAttack)
            {
                animator.SetTrigger("IceAttack");
                canAttack = false;
                isAttacking = true;
            }
            if ((!isFlipping && !isAttacking))
            {
                rigidbody.velocity = runSpeed * transform.forward;
            }
        }
        else
        {
            rigidbody.velocity = Vector3.zero;
        }

        animator.SetFloat("Speed", rigidbody.velocity.magnitude);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("FireBall"))
        {
            Destroy(collision.gameObject);
            life--;

            if (life == 0)
            {
                animator.SetTrigger("Death");
            }
            else if (life > 0)
            {
                isAttacking = false;
                isStunned = true;
                animator.SetTrigger("Hit");
            }
        }

        if (collision.gameObject.CompareTag("Player"))
        {
            if (life > 0)
            {
                collision.gameObject.GetComponent<WitcherScript>().ResetPosition();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == portalPointA || other.gameObject == portalPointB)
        {
            isFlipping = true;
            rigidbody.velocity = Vector3.zero;
            animator.SetTrigger("Turn");
        }
    }

    public void ClearFlip()
    {
        isFlipping = false;
        rigidbody.constraints = RigidbodyConstraints.None;
        transform.rotation = Quaternion.Euler(transform.localEulerAngles.x, -transform.localEulerAngles.y, transform.localEulerAngles.z);
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void ClearAttack()
    {
        isAttacking = false;
        StartCoroutine(AttackCoolDown());
    }

    public void ClearStun()
    {
        isStunned = false;
    }
    public void SpawnSnowball()
    {
        IceMagicScript iceMagic = Instantiate(snowballPrefab, projPosition.position, snowballPrefab.transform.rotation).GetComponent<IceMagicScript>();
        iceMagic.rb.velocity = Vector3.Normalize(transform.position - iceMagic.gameObject.transform.position) * iceMagic.speed;
    }

    public IEnumerator AttackCoolDown()
    {
        yield return new WaitForSeconds(attackCooldownTime);
        canAttack = true;
    }

    public void OnDeath()
    {
        Destroy(gameObject);
    }
}
