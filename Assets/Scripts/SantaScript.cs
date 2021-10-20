using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantaScript : MonoBehaviour, IDamagable
{
    [SerializeField]
    private GameObject snowballPrefab;
    [SerializeField]
    private GameObject path;
    [SerializeField]
    private GameObject projPosition;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Rigidbody rigBody;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private int life = 4;
    [SerializeField]
    private float attackCooldownTime = 1.0f;
    [SerializeField]
    private GameObject santaCoat;
    [SerializeField]
    private PlayerSpotter playerSpotter;
    private bool canAttack = true;
    private bool outsidePath = false;
    private SantaStates santaState = SantaStates.MOVING;

    private enum SantaStates
    { 
        IDLE,
        MOVING,
        FLIPPING,
        SLIDING,
        ATTACKING,
        HURT,
        DYING,
    }

    

    private void Start()
    {
        path.transform.parent = null;
    }

    private void Update()
    {
        if (santaState != SantaStates.HURT || life > 0)
        {
            //if (isPlayerInRange && santaState != SantaStates.ATTACKING && canAttack)
            //{
            //    animator.SetTrigger("IceAttack");
            //    canAttack = false;
            //    santaState = SantaStates.ATTACKING;
            //}
            if (outsidePath)
            {
                outsidePath = false;
                santaState = SantaStates.FLIPPING;
                rigBody.velocity = Vector3.zero;
                animator.SetTrigger("Turn");
            }
            else if (playerSpotter.playerWithinRange && santaState != SantaStates.ATTACKING && santaState != SantaStates.FLIPPING && !canAttack && !animator.GetCurrentAnimatorStateInfo(0).IsName("SantaIdle"))
            {
                animator.SetTrigger("Idle");
                santaState = SantaStates.IDLE;
                rigBody.velocity = Vector3.zero;
            }
            else if ((santaState == SantaStates.MOVING))
            {
                rigBody.velocity = runSpeed * transform.forward;
            }
        }
        else
        {
            rigBody.velocity = Vector3.zero;
        }

        animator.SetFloat("Speed", rigBody.velocity.magnitude);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (life > 0 && collision.gameObject.GetComponent<WitcherScript>().GetIsGround())
            {
                collision.gameObject.GetComponent<WitcherScript>().PlayerDeath();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Path") && santaState == SantaStates.MOVING && life > 0)
        {
            outsidePath = true;
        }
    }

    public void ClearFlip()
    {
        santaState = SantaStates.MOVING;
        rigBody.constraints = RigidbodyConstraints.None;
        transform.rotation = Quaternion.Euler(transform.localEulerAngles.x, -transform.localEulerAngles.y, transform.localEulerAngles.z);
        rigBody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void ClearAttack()
    {
        santaState = SantaStates.MOVING;
        StartCoroutine(AttackCoolDown());
    }

    public void ClearStun()
    {
        santaState = SantaStates.MOVING;
    }
    public void SpawnSnowball()
    {
        IceMagicScript iceMagic = Instantiate(snowballPrefab, projPosition.transform.position, snowballPrefab.transform.rotation).GetComponent<IceMagicScript>();
        //iceMagic.rb.velocity = Vector3.Normalize(transform.position - iceMagic.gameObject.transform.position) * iceMagic.speed;
        iceMagic.SetDirection(Vector3.Normalize(iceMagic.gameObject.transform.position - new Vector3(santaCoat.transform.position.x, iceMagic.gameObject.transform.position.y, iceMagic.gameObject.transform.position.z)));
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

    public void ApplyDamage(int damageTaken = 1)
    {
        life -= damageTaken; 

        if (life == 0)
        {
            animator.SetTrigger("Death");
            santaState = SantaStates.DYING;
        }
        else if (life > 0)
        {
            santaState = SantaStates.HURT;
            animator.SetTrigger("Hit");
        }
    }
}
