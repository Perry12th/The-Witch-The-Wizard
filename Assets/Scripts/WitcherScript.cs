using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitcherScript : MonoBehaviour
{
    public Rigidbody rb;
    public SpriteRenderer sr;
    public Material matr;
    public Material matl;
    public GameObject snowball;
    public GameObject fireball;
    public Transform spawnPoint;
    public Transform spawnPointLeft;
    public Vector3 checkpoint;
    public Animator anim;
    public GameObject model;

    public float speed;
    public float jumpspeed;
    public float climbSpeed;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2.5f;
    public float slipperyFriction;

    public bool lookingRight = true;
    public bool isJumping;
    public bool isGrounded = true;
    public bool isSlippery = false;
    public bool hasSnowball = false;
    public bool hasFireball = false;
    public bool canDoubleJump = false;
    public bool doubleJump = true;
    public bool isClimbing = false;
    public bool isAttacking = false;

    private void Start()
    {
        checkpoint = transform.position;
    }


    void Update()
    {
        if (!isAttacking)
        {
            if (Input.GetKey(KeyCode.A))
            {
                if (lookingRight)
                {
                    lookingRight = false;
                    FlipLeft();
                }
                rb.velocity = new Vector3(-speed, rb.velocity.y, 0);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                if (!lookingRight)
                {
                    lookingRight = true;
                    FlipRight();
                }
                rb.velocity = new Vector3(speed, rb.velocity.y, 0);
            }
            else
            {
                if (!isSlippery)
                {
                    rb.velocity = new Vector3(0, rb.velocity.y, 0);
                }
                else
                {
                    rb.velocity = new Vector3(rb.velocity.x * slipperyFriction, rb.velocity.y, 0);
                }
            }

            if (isClimbing)
            {
                rb.useGravity = false;
                isGrounded = true;
                if (Input.GetKey(KeyCode.W))
                {
                    rb.velocity = new Vector3(rb.velocity.x, climbSpeed, 0);
                }
                else
                if (Input.GetKey(KeyCode.S))
                {
                    rb.velocity = new Vector3(rb.velocity.x, -climbSpeed, 0);
                }
                else
                {
                    rb.velocity = new Vector3(rb.velocity.x, 0, 0);
                }
            }
            else
            {
                rb.useGravity = true;
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
            }

            if (Input.GetKeyDown(KeyCode.Return) && hasSnowball)
            {
                if (!lookingRight)
                {
                    GameObject newBall = Instantiate(snowball, Vector3.up * spawnPointLeft.position.y + Vector3.forward * snowball.transform.position.z + Vector3.right * spawnPointLeft.position.x, gameObject.transform.rotation);
                    newBall.transform.Rotate(Vector3.up, 180);
                }
                else
                {
                    GameObject newBall = Instantiate(snowball, Vector3.up * spawnPoint.position.y + Vector3.forward * snowball.transform.position.z + Vector3.right * spawnPoint.position.x, gameObject.transform.rotation);
                }
            }

            if (Input.GetKeyDown(KeyCode.F) && hasFireball)
            {
                anim.SetTrigger("Attack");
                isAttacking = true;
            }

            if ((Input.GetKeyDown(KeyCode.Space) && (isGrounded || doubleJump)))
            {
                if (!isGrounded && !isJumping)
                {
                    doubleJump = false;
                    isJumping = true;
                    isGrounded = false;
                    rb.velocity = (rb.velocity.x * Vector3.right) + Vector3.up * jumpspeed;
                    Invoke("ClearJump", 0.5f);
                    anim.SetTrigger("Jump");
                }
                else
                {
                    isJumping = true;
                    isGrounded = false;
                    rb.velocity = (rb.velocity.x * Vector3.right) + Vector3.up * jumpspeed;
                    Invoke("ClearJump", 0.5f);
                    anim.SetTrigger("Jump");
                }
            }
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector3.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("SpeedY", rb.velocity.y);
        anim.SetBool("Grounded", isGrounded);
    }

    public void FlipLeft()
    {
        model.transform.Rotate(Vector3.up, 180);
        sr.flipX = true;
    }

    public void FlipRight()
    {
        model.transform.Rotate(Vector3.up, 180);
        sr.flipX = false;
    }

    public void SetGrounded()
    {
        isGrounded = true;
        doubleJump = canDoubleJump;
    }

    public void ResetPosition()
    {
        transform.position = checkpoint;
    }

    public void ReleaseFireball()
    {
        if (!lookingRight)
        {
            GameObject newBall = Instantiate(fireball, Vector3.up * spawnPointLeft.position.y + Vector3.forward * fireball.transform.position.z + Vector3.right * spawnPointLeft.position.x, gameObject.transform.rotation);
            newBall.transform.Rotate(Vector3.up, 180);
        }
        else
        {
            GameObject newBall = Instantiate(fireball, Vector3.up * spawnPoint.position.y + Vector3.forward * fireball.transform.position.z + Vector3.right * spawnPoint.position.x, gameObject.transform.rotation);
        }
    }

    public void Recover()
    {
        isAttacking = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vine"))
        {
            isClimbing = true;
        }
        else
        if (other.CompareTag("Slippery"))
        {
            isSlippery = true;
        }
        else
        if (other.CompareTag("Checkpoint"))
        {
            checkpoint = new Vector3(other.transform.position.x, other.transform.position.y, transform.position.z);
        }
        else
        if (other.CompareTag("DeathZone") || other.CompareTag("Lava"))
        {
            ResetPosition();
        }
        else
        if (other.CompareTag("Snowflake"))
        {
            hasSnowball = true;
            Destroy(other.gameObject);
        }
        else
        if (other.CompareTag("FireElement"))
        {
            hasFireball = true;
            Destroy(other.gameObject);
        }
        else
        if (other.CompareTag("WindElement"))
        {
            canDoubleJump = true;
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Slippery"))
        {
            isSlippery = false;
        }
        else
        if (other.CompareTag("Vine"))
        {
            isClimbing = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("TSnowball") || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Pumpkin"))
        {
            ResetPosition();
        }

        if (collision.gameObject.CompareTag("Ground"))
        {
            SetGrounded();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (rb.velocity.y > 0f && !isJumping)
            {
                rb.velocity = (Vector3.up * 0) + rb.velocity.x * Vector3.right;
            }
            else
            {
                isGrounded = false;
            }
        }
    }

    public void ClearJump()
    {
        anim.ResetTrigger("Jump");
        isJumping = false;
    }
}
