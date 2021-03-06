using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogScript : MonoBehaviour, IDamagable
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
    private Transform groundAheadCheckPoint;
    [SerializeField]
    private Transform wallCheckPoint;
    [Tooltip("How far a groundCheck and wallCheck is performed")]
    [Range(0.1f, 3.0f)]
    [SerializeField]
    private float distanceCheck;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private Color deathColor;
    [Range(1.0f, 30.0f)]
    [SerializeField]
    private float jumpSpeed = 5.0f;
    [Range (10.0f, 80.0f)]
    [SerializeField]
    private float jumpAngle = 45.0f;
    [Tooltip("How long the frog waits in idle before preforming a jump")]
    [Range(0.5f, 5.0f)]
    [SerializeField]
    private float jumpTime = 2.0f;
    [SerializeField]
    private bool performJumps;
    private float currentTimer = 0.0f;
    private bool facingRight = true;
    private FrogStates frogState = FrogStates.IDLE;

    private enum FrogStates
    { 
        IDLE,
        JUMPING,
        HURTING,
        DYING,
    }


    void FixedUpdate()
    {
        switch (frogState)
        {
            case FrogStates.IDLE:
                currentTimer += Time.fixedDeltaTime;
                if (currentTimer >= jumpTime)
                {
                    if (!CheckGroundAhead() || CheckWall())
                    {
                        Flip();
                    }
                    PerformJump();
                }
                break;

            case FrogStates.JUMPING:
                if (CheckGround() && rb.velocity.y <= 0)
                {
                    frogState = FrogStates.IDLE;
                }
                break;

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

            if (!spikeCollider.enabled && collision.GetContact(0).point.y > transform.position.y)
            {
                
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

    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(new Vector3(0, 0, 180.0f));
    }

    private void PerformJump()
    {
        currentTimer = 0.0f;
        frogState = FrogStates.JUMPING;
        rb.velocity = jumpSpeed * (Quaternion.Euler(0, 0, facingRight? jumpAngle : -jumpAngle) * (facingRight? Vector3.right : Vector3.left));
        Debug.Log(rb.velocity);
        //Debug.Log(Quaternion.Euler(0, 0, jumpAngle) * (facingRight ? Vector3.right : Vector3.left));
        //rb.velocity = jumpSpeed * new Vector3(0.5f, 0.5f, 0);
    }

    private bool CheckGroundAhead()
    {
        return Physics.Raycast(groundAheadCheckPoint.position, Vector3.down, distanceCheck, groundLayer);
    }

    private bool CheckWall()
    {
        return Physics.Raycast(wallCheckPoint.position, facingRight ? Vector3.right : Vector3.left, distanceCheck, groundLayer);
    }

    private bool CheckGround()
    {
        return Physics.Raycast(transform.position, Vector3.down, distanceCheck, groundLayer);
    }

    public void Recover()
    {
        frogState = FrogStates.IDLE;
        currentTimer = 0.0f;
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
