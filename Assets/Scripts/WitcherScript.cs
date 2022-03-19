using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DigitalRuby.LightningBolt;

public class WitcherScript : MonoBehaviour, IDamagable
{
    [Header("Comp & Object Refs")]
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private GameObject snowball;
    [SerializeField]
    private GameObject fireball;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private GameObject model;
    [SerializeField]
    private CapsuleCollider characterCollider;
    [SerializeField]
    private AimLineScript aimLine;
    [SerializeField]
    private LightningBoltScript thunderLine;
    [SerializeField]
    private GameObject pauseMenu;
    [Header("Mats and Layers")]
    [SerializeField]
    private PhysicMaterial FrictionMaterial;
    [SerializeField]
    private PhysicMaterial FrictionLessMaterial;
    [SerializeField]
    private LayerMask aimLineCollisionMask;
    [SerializeField]
    private LayerMask groundCollisionMask;
    [Header("Movement")]
    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private float climbSpeed;
    [SerializeField]
    private float fallMultiplier = 2.5f;
    [SerializeField]
    private float lowJumpMultiplier = 2.5f;
    [SerializeField]
    private float slipperyFriction;
    [SerializeField]
    private float maxThunderAngle;
    [SerializeField]
    private float groundDistanceCheck = 0.2f;
    [SerializeField]
    private float disableGroundCheckTime = 0.2f;
    [Header("Life")]
    private int life;
    [SerializeField]
    private int maxLife;
    [Header("Mana")]
    private float manaAmount;
    [SerializeField]
    private float maxMana;
    [SerializeField]
    private float manaRegenRate = 1.0f;
    [Header("UI")]
    [SerializeField]
    private Sprite fullHeartSprite;
    [SerializeField]
    private Sprite emptyHeartSprite;
    private List<Image> hearts = new List<Image>();
    [SerializeField]
    private GameObject heartImagePrefab;
    [SerializeField]
    private TMPro.TextMeshProUGUI candyAmountText;
    private int candyAmount;
    private int candyDisplayedAmount;
    [SerializeField]
    private float candyDisplayUpdateTime = 3; // How much time the candy display takes to update to the new candy amount;
    [SerializeField]
    private GameObject healthUI;
    [SerializeField]
    private GameObject candyUI;
    [SerializeField]
    private GameObject manaUI;
    [SerializeField]
    private bool enableFullPower = false;
    private bool lookingRight = true;
    private bool isJumping;
    private bool isGrounded = true;
    private bool isSlippery = false;
    private bool hasSnowball = false;
    private bool hasFireball = false;
    private bool hasLighting = false;
    private bool doubleJumpEnabled = false;
    private bool canDoubleJump = true;
    private bool canJump = true;
    private bool isClimbing = false;
    private bool isAttacking = false;
    private bool isAiming = false;
    private bool isShootingLighting = false;
    private bool canMove = true;
    private bool isReading = false;
    //private bool canTakeDamage = true;
    private bool performGroundCheck = true;
    private Conversation conversation = null;
    private Vector3 lastAimLine;
    private Vector3 hitPoint;
    private Vector3 checkpoint;
    public Vector3 currentVelocity;


    private void Start()
    {
        checkpoint = transform.position;
        life = maxLife;
        manaAmount = 0; 
        UpdateHealthUI();
        pauseMenu.SetActive(false);

        if (enableFullPower)
        {
            hasSnowball = true;
            hasFireball = true;
            hasLighting = true;
            doubleJumpEnabled = true;
            canTakeDamage = false;
        }
    }


    private void Update()
    {
        if (canMove && !isAttacking)
        {
            RegenMana();

            if (Input.GetKey(KeyCode.Escape))
            {
                if (pauseMenu.activeSelf)
                {
                    ExitPauseMenu();
                }
                else
                {
                    EnterPauseMenu();
                }
            }
            if (Input.GetKey(KeyCode.A))
            {
                if (lookingRight && !isClimbing)
                {
                    lookingRight = false;
                    FlipLeft();
                }
                rb.velocity = new Vector3(-speed, rb.velocity.y, 0);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                if (!lookingRight && !isClimbing)
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
                    if (rb.velocity.y > 0 && isGrounded && !Input.GetKey(KeyCode.Space))
                    {
                        rb.velocity = new Vector3(0, 0, 0);
                    }
                    else
                    {
                        rb.velocity = new Vector3(0, rb.velocity.y, 0);
                    }
                }
                else
                {
                    rb.velocity = new Vector3(rb.velocity.x * slipperyFriction, rb.velocity.y, 0);
                }
            }

            if (isClimbing)
            {
                rb.useGravity = false;
                if (Input.GetKey(KeyCode.W))
                {
                    rb.velocity = new Vector3(rb.velocity.x, climbSpeed, 0);
                    if (!lookingRight)
                    {
                        FlipRight();
                        lookingRight = true;
                    }
                }
                else
                if (Input.GetKey(KeyCode.S))
                {
                    rb.velocity = new Vector3(rb.velocity.x, -climbSpeed, 0);
                    if (!lookingRight)
                    {
                        FlipRight();
                        lookingRight = true;
                    }
                }
                else
                {
                    rb.velocity = new Vector3(rb.velocity.x, 0, 0);
                    if (!lookingRight && !isGrounded)
                    {
                        FlipRight();
                        lookingRight = true;
                    }
                }
            }
            else
            {
                rb.useGravity = true;
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
            }

            if (Input.GetKeyDown(KeyCode.Return) && hasSnowball)
            {
                anim.speed = 2.0f;
                isAttacking = true;
                anim.SetTrigger("IceSpell");
                characterCollider.material = FrictionMaterial;
            }

            if (Input.GetKeyDown(KeyCode.F) && hasFireball)
            {
                //characterCollider.material = FrictionMaterial;
                anim.SetTrigger("FireSpell");
                characterCollider.material = FrictionMaterial;
                isAttacking = true;
            }

            if (Input.GetKeyDown(KeyCode.T) && hasLighting && isGrounded)
            {
                aimLine.gameObject.SetActive(true);
                isAiming = true;
                canMove = false;
            }

            if ((Input.GetKeyDown(KeyCode.Space) && (isGrounded || canDoubleJump)))
            {
                if (!isGrounded)
                {
                    StopAllCoroutines();
                    StartCoroutine(DisableGroundCheck());
                    canDoubleJump = false;
                    isJumping = true;
                    rb.velocity = (rb.velocity.x * Vector3.right) + Vector3.up * jumpSpeed;
                    //rb.velocity += Vector3.up * jumpSpeed;
                    anim.SetTrigger("DoubleJump");
                }
                else
                {
                    StopAllCoroutines();
                    StartCoroutine(DisableGroundCheck());
                    isJumping = true;
                    rb.velocity = (rb.velocity.x * Vector3.right) + Vector3.up * jumpSpeed;
                    anim.SetTrigger("Jump");
                }
            }

            if (isGrounded && conversation != null && conversation?.DialogueV2s.Length > 0 && !isReading)
            {
                rb.velocity = Vector3.zero;
                canMove = false;
                isReading = true;
                HideUI();
                DialogueManager.instance.StartConversation(conversation);
                conversation = null;
            }  
        }

        if (isReading && Input.GetKeyDown(KeyCode.Return))
        {
             DialogueManager.instance.AdvanceDialouge();
             if (!DialogueManager.instance.isOpen)
             {
                 ShowUI();
                 isReading = false;
                 canMove = true;
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
                characterCollider.material = FrictionMaterial; 
                anim.SetBool("ThunderSpell", true);
                isAiming = false;
            }
        }

        
        
    }

    private void FixedUpdate()
    {
        if (performGroundCheck)
        {
            GroundCheck();
        }

        if (rb.velocity.y < 0 && !isGrounded)
        {
            rb.velocity += Vector3.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump") && !isGrounded)
        {
            rb.velocity += Vector3.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("SpeedY", Mathf.Abs(rb.velocity.y));
        anim.SetBool("Grounded", isGrounded);
    }
    public void FlipLeft()
    {
        lookingRight = false;
        model.transform.Rotate(Vector3.up, 180);
    }

    public void FlipRight()
    {
        lookingRight = true;
        model.transform.Rotate(Vector3.up, 180);
    }

    public void SetGrounded(bool ground)
    {
        isGrounded = ground;
        if (ground)
        {
            isJumping = !ground;
            anim.ResetTrigger("Jump");
            anim.ResetTrigger("DoubleJump");
            canJump = true;
            canDoubleJump = doubleJumpEnabled;
        }
    }

    private void GroundCheck()
    {
        RaycastHit raycastHit;
        // Physics.BoxCast(groundCheckBox.bounds.center, groundCheckBox.bounds.size, Vector3.down, out raycastHit, Quaternion.identity, groundDistanceCheck, groundCollisionMask);
        Physics.Raycast(characterCollider.bounds.center, Vector3.down, out raycastHit, groundDistanceCheck, groundCollisionMask);
        if (raycastHit.collider != null)
        {
            SetGrounded(true);
            //Quaternion slopeRotation = Quaternion.FromToRotation(Vector3.up, raycastHit.normal);
            //Vector3 adjustedVelocity = slopeRotation * rb.velocity;
            //if (adjustedVelocity.y < Mathf.Epsilon)
            //{
            //    rb.velocity = adjustedVelocity;
            //}
            currentVelocity = rb.velocity;
            if (!isClimbing && Mathf.Abs(rb.velocity.x) > 1.0f)
            {
                Vector3 adjustedVelocity = Vector3.ProjectOnPlane(rb.velocity, raycastHit.normal);
                rb.velocity = adjustedVelocity;
            }

        }
        else
        {
            SetGrounded(false);        
        }
    }

    public void PlayerDeath()
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("SuzyDeath"))
        {
            DialogueManager.instance.EndDialogue();
            anim.SetTrigger("Death");
            canMove = false;
            isReading = false;
        }
    }

    public void ResetPlayerToCheckpoint()
    {
        transform.position = checkpoint;
        RecoverHeath(maxLife);
        canMove = true;
    }

    public void ReleaseFireball()
    {
        GameObject newBall = Instantiate(fireball, spawnPoint.transform.position, lookingRight ? fireball.transform.rotation : Quaternion.Euler(0, 180.0f, 0));
    }

    public void Recover()
    {
        isAttacking = false;
        characterCollider.material = FrictionLessMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vine"))
        {
            isClimbing = true;
            anim.SetBool("IsClimbing", true);
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
            ResetPlayerToCheckpoint();
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
            doubleJumpEnabled = true;
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
            anim.SetBool("IsClimbing", false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Lava"))
        {
            ApplyDamage(maxLife);
        }

        if (collision.gameObject.CompareTag("TSnowball"))
        {
            ApplyDamage();
        }
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
        // Check if the mousePosition is in front or behind the player
        if (mousePos.x < transform.position.x && lookingRight)
        {
            FlipLeft();
        }
        else if (mousePos.x > transform.position.x && !lookingRight)
        {
            FlipRight();
        }

        //mousePos.z = model.transform.position.z;
        //mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        //Debug.Log(mousePos);

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

        // Check if the angle is in the maxThunderAngle
        if (angleBetween <= maxThunderAngle)
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

    private IEnumerator DisableGroundCheck()
    {
        performGroundCheck = false;
        yield return new WaitForSeconds(disableGroundCheckTime);
        performGroundCheck = true;
    }

    private void UpdateHealthUI()
    {
        if (hearts.Count != maxLife)
        {
            int heartDifference = Mathf.Abs(hearts.Count - maxLife);
            bool requiredMoreHearts = hearts.Count < maxLife;;
            for (int i = 0;i < heartDifference; i++)
            {
                if (requiredMoreHearts) // We need to add hearts to the UI
                {
                    Image newHeart = Instantiate(heartImagePrefab).GetComponent<Image>();
                    newHeart.gameObject.transform.SetParent(healthUI.transform, false);
                    hearts.Add(newHeart);
                }
                else // We need to remove hearts from the UI
                {
                    Destroy(hearts[hearts.Count - 1]);
                }
            }
        }

        for (int i = 0; i < maxLife; i++)
        {
            if (i < life)
            {
                hearts[i].sprite = fullHeartSprite;
            }
            else
            {
                hearts[i].sprite = emptyHeartSprite;
            }

            if (i < maxLife)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    private void RecoverHeath(int amount)
    {
        life += amount;
        life = Mathf.Clamp(life, 0, maxLife);
        UpdateHealthUI();
    }

    public void FireLighting()
    {
        thunderLine.gameObject.SetActive(true);
        thunderLine.StartPosition = thunderLine.gameObject.transform.position;
        thunderLine.EndPosition = hitPoint;

        // Perform Raycast
        RaycastHit hit;
        if (Physics.Raycast(aimLine.transform.position, lastAimLine, out hit, Vector2.Distance(thunderLine.transform.position, hitPoint) * 1.2f, aimLineCollisionMask))
        {
            IElectrical electrical = hit.collider.gameObject.GetComponent<IElectrical>();
            if (electrical != null)
            {
                electrical.OnPowered();
                return;
            }
            IDamagable damagable = hit.collider.gameObject.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.ApplyDamage();
                return;
            }
        }
    }

    public void RecoverLighting()
    {
        thunderLine.gameObject.SetActive(false);
        characterCollider.material = FrictionLessMaterial;
        isShootingLighting = false;
        canMove = true;
    }

    public void FireSnowBall()
    {
        GameObject newBall = Instantiate(snowball, spawnPoint.transform.position, lookingRight? snowball.transform.rotation : Quaternion.Euler(0, 180.0f, 0));
    }

    public void RecoverSnowBall()
    {
        anim.speed = 1.0f;
        isAttacking = false;
        characterCollider.material = FrictionLessMaterial;
    }

    public void SetDialogue(Conversation newConversation)
    {
        conversation = newConversation;
    }

    public Rigidbody GetRigidBody()
    {
        return rb;
    }

    public bool GetIsGround()
    {
        return isGrounded;
    }

    public GameObject GetPlayerModel()
    {
        return model;
    }

    public bool GetIsLookingRight()
    {
        return lookingRight;
    }

    public void ExitPauseMenu()
    {
        GameManager.instance.UnPauseGame();
        ShowUI();
        pauseMenu.SetActive(false);
        canMove = true;
    }

    public void EnterPauseMenu()
    {
        GameManager.instance.PauseGame();
        HideUI();
        pauseMenu.SetActive(true);
        canMove = false;
    }

    public void ApplyDamage(int damageTaken = 1)
    {
        life -= damageTaken;
        life = Mathf.Clamp(life, 0, maxLife);
        UpdateHealthUI();

        if (life <= 0)
        {
            PlayerDeath();
        }
    }

    public void GainCandy(int candyIncrease)
    {
        Debug.Log("GainCandy" + candyIncrease);
        StopCoroutine(IncreaseDisaplyedCandyAmount());
        candyAmount += candyIncrease;
        if (candyIncrease <= 5)
        {
            candyDisplayedAmount = candyAmount;
            candyAmountText.text = candyDisplayedAmount.ToString();
        }
        else
        {
            StartCoroutine(IncreaseDisaplyedCandyAmount());
        }
    }

    private IEnumerator IncreaseDisaplyedCandyAmount()
    {
        float timer = 0;
        int intialCandyDisplayAmount = candyDisplayedAmount;

        while (timer < candyDisplayUpdateTime)
        {
            timer += Time.deltaTime;
            candyDisplayedAmount = (int)Mathf.Lerp(intialCandyDisplayAmount, candyAmount, timer / candyDisplayUpdateTime);
            candyAmountText.text = candyDisplayedAmount.ToString();
            yield return new WaitForEndOfFrame();
        }
        
    }

    private void RegenMana()
    {
        manaAmount += manaRegenRate * Time.deltaTime;
        manaAmount = Mathf.Clamp(manaAmount, 0, maxMana);
    }

    public float GetManaAmount()
    {
        return manaAmount;
    }

    public float GetMaxMana()
    {
        return maxMana; 
    }

    private void HideUI()
    {
        manaUI.SetActive(false);
        healthUI.SetActive(false);
        candyUI.SetActive(false);
    }

    private void ShowUI()
    {
        manaUI.SetActive(true);
        healthUI.SetActive(true);   
        candyUI.SetActive(true);
    }
}
