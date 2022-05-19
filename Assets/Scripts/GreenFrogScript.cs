using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenFrogScript : MonoBehaviour, IDamagable
{
    [SerializeField]
    private Animator animator;
    //[SerializeField]
    //private Rigidbody rb;
    [SerializeField]
    private Collider bodyCollider;
    [SerializeField]
    private Collider spikeCollider;
    [SerializeField]
    private Renderer bodyRenderer;
    [SerializeField]
    private Renderer spikeRenderer;
    [Tooltip("How far a groundCheck and wallCheck is performed")]
    [Range(0.1f, 3.0f)]
    //[SerializeField]
    //private float distanceCheck;
    //[SerializeField]
    //private LayerMask groundLayer;
    [SerializeField]
    private Color deathColor;
    //[Range(1.0f, 30.0f)]
    //[SerializeField]
    //private float jumpSpeed = 5.0f;
    [Tooltip("How long the frog waits in idle before preforming a jump")]
    [Range(0.5f, 30.0f)]
    [SerializeField]
    private float jumpTime = 2.0f;
    [SerializeField]
    private bool performJumps;
    private bool playerInRange;
    private float currentTimer = 0.0f;
    private float startingYPositon;
    private FrogStates frogState = FrogStates.IDLE;

    private enum FrogStates
    {
        IDLE,
        JUMPING,
        HURTING,
        DYING,
    }

    public void Start()
    {
        startingYPositon = transform.position.y;
    }



    void FixedUpdate()
    {
        switch (frogState)
        {
            case FrogStates.IDLE:
                if (performJumps)
                {
                    currentTimer += Time.fixedDeltaTime;
                    if (currentTimer >= jumpTime)
                    {
                        PerformJump();
                    }
                }
                break;

            //case FrogStates.JUMPING:
            //    if (CheckGround() && rb.velocity.y <= 0)
            //    {
            //        frogState = FrogStates.IDLE;
            //    }
            //    break;

        }

        //if (frogState == FrogStates.IDLE && performJumps)
        //{
        //    currentTimer += Time.fixedDeltaTime;
        //    if (currentTimer >= jumpTime)
        //    {
        //        if (!CheckGroundAhead() || CheckWall())
        //        {
        //            Flip();
        //        }
        //        PerformJump();
        //    }
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && frogState != FrogStates.DYING)
        {

            if (!spikeCollider.enabled && collision.GetContact(0).point.y > (transform.position.y + bodyCollider.bounds.size.y/2))
            {

                ApplyDamage();
            }
            else
            {
                collision.gameObject.GetComponent<WitcherScript>().ApplyDamage();
            }

        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && frogState == FrogStates.IDLE)
        {
            playerInRange = true;
            PerformJump();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && frogState != FrogStates.DYING)
        {
            playerInRange = false;
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

    private void PerformJump()
    {
        currentTimer = 0.0f;
        frogState = FrogStates.JUMPING;
        animator.SetTrigger("Jump");
        //Debug.Log(Quaternion.Euler(0, 0, jumpAngle) * (facingRight ? Vector3.right : Vector3.left));
        //rb.velocity = jumpSpeed * new Vector3(0.5f, 0.5f, 0);
    }

    private void AdjustPosition()
    {
        //transform.position = new Vector3(transform.position.x, startingYPositon, transform.position.z);w
    }



    //private bool CheckGround()
    //{
    //    return Physics.Raycast(transform.position, Vector3.down, distanceCheck, groundLayer);
    //}

    public void Recover()
    {
        //if (playerInRange)
        //{
        //    PerformJump();
        //}

        //else
        //{
        //    frogState = FrogStates.IDLE;
        //    animator.SetTrigger("Idle");
        //    currentTimer = 0.0f;
        //}
        frogState = FrogStates.IDLE;
        animator.SetTrigger("Idle");
        currentTimer = 0.0f;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

    public void SpawnSmokePoof()
    {
        EffectsManager.instance.SpawnSmokePoof(transform);
    }
}

