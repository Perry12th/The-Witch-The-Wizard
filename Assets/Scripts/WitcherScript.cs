using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.LightningBolt;

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
    public CapsuleCollider FeetCollider;
    public PhysicMaterial FrictionMaterial;
    public PhysicMaterial FrictionLessMaterial;
    public LayerMask PlayerCollisionMask;

    public AimLineScript aimLine;
    public LightningBoltScript thunderLine;
    public float thunderAngle;
    public Vector3 lastAimLine;
    public Vector3 hitPoint;
    public LayerMask aimLineCollisionMask;

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
    public bool hasLighting = false;
    public bool canDoubleJump = false;
    public bool doubleJump = true;
    public bool isClimbing = false;
    public bool isAttacking = false;
    public bool isAiming = false;
    public bool isShootingLighting = false;

    private void Start()
    {
        checkpoint = transform.position;
    }


    void Update()
    {
        if (!isAttacking && !isAiming && !isShootingLighting)
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
                FeetCollider.material = FrictionMaterial;
                anim.SetTrigger("Attack");
                isAttacking = true;
            }

            //if (Input.GetKeyDown(KeyCode.F) && hasFireball && !isGrounded)
            //{
            //    isAttacking = true;
            //    ReleaseFireball();
            //}

            if (Input.GetKeyDown(KeyCode.T) && hasLighting && isGrounded)
            {
                aimLine.gameObject.SetActive(true);
                isAiming = true;
            }

            if ((Input.GetKeyDown(KeyCode.Space) && (isGrounded || doubleJump)))
            {
                if (!isGrounded)
                {
                    doubleJump = false;
                    isJumping = true;
                    isGrounded = false;
                    rb.velocity = (rb.velocity.x * Vector3.right) + Vector3.up * jumpspeed;
                    Invoke("ClearJump", 0.5f);
                    anim.SetTrigger("DoubleJump");
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

        if (isAiming)
        {
            rb.velocity = Vector3.zero;
            AimingThunder();
            if (Input.GetKeyUp(KeyCode.T))
            {
                isShootingLighting = true;
                aimLine.gameObject.SetActive(false);
                anim.SetBool("Attack2", true);
                isAiming = false;
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

        if (!isShootingLighting)
        {
            anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
            anim.SetFloat("SpeedY", rb.velocity.y);
            anim.SetBool("Grounded", isGrounded);
        }
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

    public void SetGrounded(bool ground)
    {
        isGrounded = ground;
        if (ground)
        {
            doubleJump = canDoubleJump;
        }
    }

    //public void SetIsGrounded()
    //{
    //    float distanceToPoints = FeetCollider.height / 2 - FeetCollider.radius;

    //    Vector3 startPoint = FeetCollider.center + Vector3.up * distanceToPoints;
    //    Vector3 endPoint = FeetCollider.center - Vector3.down * distanceToPoints;

    //    RaycastHit raycast = new RaycastHit();
            
    //    Physics.CapsuleCast(startPoint, endPoint, FeetCollider.radius, Vector3.down,  out raycast, 0.1f, PlayerCollisionMask);

    //    if (raycast.collider != null)
    //    {
    //        Debug.Log(raycast.collider.name)
    //    }
    //    if (raycast.collider != null && raycast.collider.CompareTag("Ground"))
    //    {
    //        isGrounded = true;
    //    }
    //    else
    //    {
    //        isGrounded = false;
    //    }
    //}

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
        FeetCollider.material = FrictionLessMaterial;
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
        if (other.CompareTag("DeathZone"))
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
        else 
        if (other.CompareTag("ThunderElement"))
        {
            hasLighting = true;
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
            if (Vector2.Angle(Vector2.up, collision.GetContact(0).normal) <= 45f)
            {
                SetGrounded(true);
            }
        }
        if(collision.gameObject.CompareTag("Lava"))
        {
            ResetPosition();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (Vector2.Angle(Vector2.up, collision.GetContact(0).normal) <= 80f)
            {
                SetGrounded(true);
            }
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
                SetGrounded(false);
            }
        }
    }
    


    public void ClearJump()
    {
        anim.ResetTrigger("Jump");
        isJumping = false;
    }

    private void AimingThunder()
    {
        // Get the mouse position of the world via a plane create on the witch model
        Plane playerPlane = new Plane(Vector3.forward, new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, model.transform.position.z));
        Vector3 mousePos = Input.mousePosition;
        Ray ray2 = Camera.main.ScreenPointToRay(mousePos);
        float mouseDistance = 0f;
        if (playerPlane.Raycast(ray2, out mouseDistance))
        {
            mousePos = ray2.GetPoint(mouseDistance);
        }
        else
        {
            mousePos = model.transform.forward * 1.5f;
        }
        //mousePos.z = model.transform.position.z;
        //mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Debug.Log(mousePos);

        // Get the forward vector of the player
        Vector3 forwardVector = model.transform.forward;
        if (lastAimLine == Vector3.zero)
            lastAimLine = forwardVector;

        // Set aim line start point
        aimLine.SetStartPoint(aimLine.transform.position);

        //Calculate shooting line
        Vector3 thunderShootingLine = mousePos - aimLine.transform.position;
        thunderShootingLine.Normalize();
        //Debug.Log(thunderShootingLine);

        // Get the angle between forward player and shooting line
        float angleBetween = Vector2.Angle(forwardVector, thunderShootingLine);
        Debug.Log(angleBetween);
        if (angleBetween <= thunderAngle)
        {
            lastAimLine = thunderShootingLine;
        }          

        // Perform Ray hit against the edges of the screen
        Ray ray = new Ray(aimLine.transform.position, lastAimLine);
        float currentMinDistance = float.MaxValue;
        var planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
        for (var i = 0; i < 4; i++)
        {
            // Raycast against the plane
            if (planes[i].Raycast(ray, out var distance))
            {
                // Since a plane is mathematical infinite
                // what you would want is the one that hits with the shortest ray distance
                if (distance < currentMinDistance)
                {
                    hitPoint = ray.GetPoint(distance);
                    currentMinDistance = distance;
                }
            }
        }

        // Perform Raycast to check if there's any object between the player and the edge of the screen
        RaycastHit hit;
        if (Physics.Raycast(aimLine.transform.position, lastAimLine, out hit, currentMinDistance, aimLineCollisionMask))
        {
            hitPoint = hit.point;
        }
             
         aimLine.SetEndPoint(hitPoint);    
    }

    public void FireLighting()
    {
        thunderLine.gameObject.SetActive(true);
        thunderLine.StartPosition = thunderLine.gameObject.transform.position;
        thunderLine.EndPosition = hitPoint;

        // Perform Raycast
        RaycastHit hit;
        if (Physics.Raycast(aimLine.transform.position, lastAimLine, out hit, Vector2.Distance(thunderLine.transform.position, hitPoint), aimLineCollisionMask))
        {
            IElectrical electrical = hit.collider.gameObject.GetComponent<IElectrical>();
            if (electrical != null)
            {
                electrical.OnPowered();
            }
        }
    }

    public void RecoverLighting()
    {
        thunderLine.gameObject.SetActive(false);
        isShootingLighting = false;
    }
}
