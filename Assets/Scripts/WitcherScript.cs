using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
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
    private GameObject charmEffect;
    [SerializeField]
    private GameObject lightingFlash;
    [SerializeField]
    private GameObject suzyCat;
    [SerializeField]
    private Transform projectileSpawnPoint;
    [SerializeField]
    private Transform smokePoofSpawnPoint;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Animator suzyAnimator;
    [SerializeField]
    private Animator catAnimator;
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
    [SerializeField]
    private CameraScript cameraScript;
    [SerializeField]
    private List<Renderer> renderers;
    [SerializeField] 
    private List<Renderer> catRenderers;
    [Header("Mats and Layers")]
    [SerializeField]
    private PhysicMaterial frictionMaterial;
    [SerializeField]
    private PhysicMaterial frictionLessMaterial;
    [SerializeField]
    private LayerMask aimLineCollisionMask;
    [SerializeField]
    private LayerMask groundCollisionMask;
    [SerializeField]
    private LayerMask enemyElecticalCollisionMask;
    [SerializeField]
    private int creatureMask;
    [SerializeField]
    private int ghostMask;
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
    [SerializeField]
    private float ghostDuration;
    [SerializeField]
    private float flashDuration;
    [Header("Mana")]
    private float manaAmount;
    [SerializeField]
    private float maxMana;
    [SerializeField]
    private float manaRegenRate = 1.0f;
    [Header("Spells & ManaCost")]
    [SerializeField]
    private float fireBallSpellCost = 20.0f;
    [SerializeField]
    private float thunderSpellCost = 20.0f;
    [SerializeField]
    private float charmSpellCost = 20.0f;
    [SerializeField]
    private float snowBallCost = 20.0f;
    [SerializeField]
    private float catSpellCost = 20.0f;
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
    private bool hasCharm = false;
    private bool hasCat = false;
    private bool inCatForm = false;
    private bool doubleJumpEnabled = false;
    private bool canDoubleJump = true;
    //private bool canJump = true;
    private bool onVine = false;
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
    private static readonly int OnVine = Animator.StringToHash("OnVine");
    private static readonly int IsClimbing = Animator.StringToHash("IsClimbing");

    public enum SpellType
    { 
        SNOWBALL,
        FIREBALL,
        LIGHTING,
        CHARM,
        DOUBLEJUMP,
    }

    private void Start()
    {
        checkpoint = transform.position;
        life = maxLife;
        manaAmount = 0;
        anim = suzyAnimator;

        UpdateHealthUI();
        pauseMenu.SetActive(false);

        if (enableFullPower)
        {
            hasSnowball = true;
            hasFireball = true;
            hasLighting = true;
            hasCharm = true;
            doubleJumpEnabled = true;
            hasCat = true;
            //canTakeDamage = false;
        }
    }


    private void Update()
    {
        if (!GameManager.instance.gameIsPaused && Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (candyAmountText.enabled)
            {
                HideUI();
            }
            else
            {
                ShowUI();
            }
        }
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
                if (lookingRight && !onVine)
                {
                    lookingRight = false;
                    FlipLeft();
                }
                
                rb.velocity = new Vector3(-speed, rb.velocity.y, 0);
            }
            else if (Input.GetKey(KeyCode.D))
            {
                if (!lookingRight && !onVine)
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
                        //rb.velocity = new Vector3(0, 0, 0);
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

            if (onVine)
            {
                rb.useGravity = false;
                if (Input.GetKey(KeyCode.W))
                {
                    rb.velocity = new Vector3(rb.velocity.x, climbSpeed, 0);
                    anim.SetBool(IsClimbing, true);
                    if (!lookingRight)
                    {
                        FlipRight();
                        lookingRight = true;
                    }
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    rb.velocity = new Vector3(rb.velocity.x, -climbSpeed, 0);
                    anim.SetBool(IsClimbing, true);
                    if (!lookingRight)
                    {
                        FlipRight();
                        lookingRight = true;
                    }
                }
                else
                {
                    rb.velocity = new Vector3(rb.velocity.x, 0, 0);
                    anim.SetBool(IsClimbing, false);
                    if (!lookingRight && !isGrounded)
                    {
                        FlipRight();
                        lookingRight = true;
                    }
                }
            }
            else
            {
                if (canMove)
                {
                    rb.useGravity = true;
                }
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, 0);
            }

            if (Input.GetKeyDown(KeyCode.Return) && hasSnowball && manaAmount >= snowBallCost)
            {
                anim.speed = 2.0f;
                isAttacking = true;
                anim.SetTrigger("IceSpell");
                characterCollider.material = frictionMaterial;
            }

            if (Input.GetKeyDown(KeyCode.C) && hasCharm && manaAmount >= charmSpellCost)
            {
                isAttacking = true;
                anim.SetTrigger("CharmSpell");
                characterCollider.material = frictionMaterial;
            }

            if (Input.GetKeyDown(KeyCode.K) && hasCat && manaAmount >= catSpellCost)
            {
                if (!suzyCat.activeSelf)
                {
                    characterCollider.material = frictionMaterial;
                    anim.SetTrigger("CatSpell");
                    isAttacking = true;
                }
                else
                {
                    TransformFromCat();
                }
                
            }

            if (Input.GetKeyDown(KeyCode.F) && hasFireball && manaAmount >= fireBallSpellCost)
            {
                //characterCollider.material = FrictionMaterial;
                anim.SetTrigger("FireSpell");
                characterCollider.material = frictionMaterial;
                isAttacking = true;
            }

            if (Input.GetKeyDown(KeyCode.T) && hasLighting && isGrounded && manaAmount >= thunderSpellCost)
            {
                aimLine.gameObject.SetActive(true);
                isAiming = true;
                AllowMovement(false);
            }

            if ((Input.GetKeyDown(KeyCode.Space) && (isGrounded || canDoubleJump)))
            {
                if (!isGrounded && !inCatForm)
                {
                    StopCoroutine(DisableGroundCheck(disableGroundCheckTime));
                    StartCoroutine(DisableGroundCheck(disableGroundCheckTime));
                    canDoubleJump = false;
                    isJumping = true;
                    rb.velocity = (rb.velocity.x * Vector3.right) + Vector3.up * jumpSpeed;
                    anim.SetTrigger("DoubleJump");
                }
                else if (!inCatForm)
                {
                    StopCoroutine(DisableGroundCheck(disableGroundCheckTime));
                    StartCoroutine(DisableGroundCheck(disableGroundCheckTime));
                    isJumping = true;
                    rb.velocity = (rb.velocity.x * Vector3.right) + Vector3.up * jumpSpeed;
                    anim.SetTrigger("Jump");
                }
                else if (isGrounded && inCatForm)
                {
                    StopCoroutine(DisableGroundCheck(disableGroundCheckTime));
                    StartCoroutine(DisableGroundCheck(0.40f));
                    anim.SetTrigger("Jump");
                    isJumping = true;
                    StartCoroutine(SuzyCatJump());
                }
            }

            if (isGrounded && conversation != null && conversation?.DialogueV2s.Length > 0 && !isReading)
            {
                rb.velocity = Vector3.zero;
                AllowMovement(false);
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
                //ShowUI();
                isReading = false;
                AllowMovement(true);
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
                characterCollider.material = frictionMaterial; 
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
        anim.SetFloat("SpeedY", rb.velocity.y);
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
            anim.ResetTrigger("DoubleJump");
            canDoubleJump = doubleJumpEnabled;
        }
    }

    private void GroundCheck()
    {
        RaycastHit raycastHit;
        Physics.Raycast(characterCollider.bounds.center, Vector3.down, out raycastHit, groundDistanceCheck, groundCollisionMask);
        if (raycastHit.collider != null)
        {
            SetGrounded(true);
            currentVelocity = rb.velocity;
            if (!onVine && Mathf.Abs(rb.velocity.x) > 1.0f)
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
            StopAllCoroutines();
            FinishGhosting();
            TransformFromCat();
            DialogueManager.instance.EndDialogue();
            cameraScript.ResetCamera(false);
            anim.SetTrigger("Death");
            HideUI();
            AllowMovement(false);
            isReading = false;
        }
    }

    public void ResetPlayerToCheckpoint()
    {
        transform.position = checkpoint;
        RecoverHeath(maxLife);
        AllowMovement(true);
        //ShowUI();
    }

    public void ReleaseFireball()
    {
        DrainMana(fireBallSpellCost);
        Instantiate(fireball, projectileSpawnPoint.transform.position, lookingRight ? fireball.transform.rotation : Quaternion.Euler(0, 180.0f, 0));
    }

    public void ReleaseCharm()
    {
        DrainMana(charmSpellCost);
        CharmPoofScript charmPoof = Instantiate(charmEffect, projectileSpawnPoint.transform.position, projectileSpawnPoint.transform.rotation).GetComponent<CharmPoofScript>();
        charmPoof.setFacing(lookingRight ? true : false);
    }   

    public void Recover()
    {
        isAttacking = false;
        characterCollider.material = frictionLessMaterial;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Vine") && !inCatForm)
        {
            onVine = true;
            anim.SetBool(OnVine, true);
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
            onVine = false;
            anim.SetBool(OnVine, false);
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
                    hitPoint = ray.GetPoint(distance * 1.2f);
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

    private IEnumerator DisableGroundCheck(float disableTime)
    {
        performGroundCheck = false;
        yield return new WaitForSeconds(disableTime);
        performGroundCheck = true;
    }

    private void UpdateHealthUI()
    {
        if (!candyAmountText.enabled)
        {
            return;
        }

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
        DrainMana(thunderSpellCost);

        thunderLine.StartPosition = thunderLine.gameObject.transform.position;
        thunderLine.EndPosition = hitPoint;
        thunderLine.gameObject.SetActive(true);
        Instantiate(lightingFlash, thunderLine.StartPosition, new Quaternion());
        // Perform Raycast to find any damagable or electrical object in the pathway
        RaycastHit[] hits;
        hits = Physics.RaycastAll(aimLine.transform.position, lastAimLine, Vector2.Distance(thunderLine.transform.position, hitPoint), enemyElecticalCollisionMask);
        foreach (RaycastHit hit in hits)
        {
            EffectsManager.instance.SpawnLightingBlast(hit.point, new Quaternion(), Vector3.one);
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
        characterCollider.material = frictionLessMaterial;
        isShootingLighting = false;
        AllowMovement(true);
    }

    public void FireSnowBall()
    {
        DrainMana(snowBallCost);
        GameObject newBall = Instantiate(snowball, projectileSpawnPoint.transform.position, lookingRight? snowball.transform.rotation : Quaternion.Euler(0, 180.0f, 0));
    }

    public void RecoverSnowBall()
    {
        anim.speed = 1.0f;
        isAttacking = false;
        characterCollider.material = frictionLessMaterial;
    }

    public void TransformToCat()
    {
        suzyCat.SetActive(true);
        EnableSuzyRenderers(false);
        anim = catAnimator;
        isAttacking = false;
        inCatForm = true;
    }

    public void TransformFromCat()
    {
        SpawnCatSpellPoof();
        EnableSuzyRenderers(true);
        suzyCat.SetActive(false);
        anim = suzyAnimator;
        isAttacking = false;
        inCatForm = false;
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
        //ShowUI();
        pauseMenu.SetActive(false);
        AllowMovement(true);
    }

    public void EnterPauseMenu()
    {
        GameManager.instance.PauseGame();
        HideUI();
        pauseMenu.SetActive(true);
        AllowMovement(false);
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
        else 
        {
            StartGhosting();
        }
    }

    public void GainCandy(int candyIncrease)
    {
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

    private IEnumerator GhostFlashing()
    {
        float timer = 0;

        while (timer < ghostDuration)
        {
            timer += flashDuration;
            if (inCatForm)
            {
                EnableSuzyCatRenders(false);
            }
            else
            {
                EnableSuzyRenderers(false);
            }
            yield return new WaitForSeconds(flashDuration);
            timer += flashDuration;
            if (inCatForm)
            {
                EnableSuzyCatRenders(true);
            }
            else
            {
                EnableSuzyRenderers(true);
            }
            yield return new WaitForSeconds(flashDuration);
        }
        FinishGhosting();
    }

    private void EnableSuzyRenderers(bool enabled)
    {
        foreach(Renderer renderer in renderers)
        {
            renderer.gameObject.SetActive(enabled);
        }
    }

    private void EnableSuzyCatRenders(bool enabled)
    {
        foreach (Renderer renderer in catRenderers)
        {
            renderer.gameObject.SetActive(enabled);
        }
    }

    private void StartGhosting()
    {
        gameObject.layer = ghostMask;
        StartCoroutine(GhostFlashing());
    }

    private void FinishGhosting()
    {
        gameObject.layer = creatureMask;
        if (inCatForm)
        {
            EnableSuzyCatRenders(true);
        }
        else
        {
            EnableSuzyRenderers(true);
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

    public void GainMana(float manaGain)
    {
        manaAmount += manaGain;
        manaAmount = Mathf.Clamp(manaAmount, 0, maxMana);
    }
    public void DrainMana(float manaCost)
    {
        manaUI.GetComponent<ManaUIScript>().InitLostManaBar();
        manaAmount -= manaCost;
        manaAmount = Mathf.Clamp(manaAmount, 0, maxMana);
    }

    private void HideUI()
    {
        foreach(Image image in manaUI.GetComponentsInChildren<Image>())
        {
            image.enabled = false;
        }
        foreach (RawImage image in manaUI.GetComponentsInChildren<RawImage>())
        {
            image.enabled = false;
        }
        foreach (Image image in healthUI.GetComponentsInChildren<Image>())
        {
            image.enabled = false;
        }
        foreach (Image image in candyUI.GetComponentsInChildren<Image>())
        {
            image.enabled = false;
        }
        foreach (TMPro.TextMeshProUGUI text in candyUI.GetComponentsInChildren<TMPro.TextMeshProUGUI>())
        {
            text.enabled = false;
        }
    }

    private void ShowUI()
    {
        foreach (Image image in manaUI.GetComponentsInChildren<Image>())
        {
            image.enabled = true;
        }
        foreach (RawImage image in manaUI.GetComponentsInChildren<RawImage>())
        {
            image.enabled = true;
        }
        foreach (Image image in healthUI.GetComponentsInChildren<Image>())
        {
            image.enabled = true;
        }
        foreach (Image image in candyUI.GetComponentsInChildren<Image>())
        {
            image.enabled = true;
        }
        foreach (TMPro.TextMeshProUGUI text in candyUI.GetComponentsInChildren<TMPro.TextMeshProUGUI>())
        {
            text.enabled = true;
        }
    }

    public void EnableSpell(SpellType spell)
    {
        switch (spell)
        {
            case SpellType.SNOWBALL:
                hasSnowball = true;
                break;
            case SpellType.FIREBALL:
                hasFireball = true;
                break;
            case SpellType.LIGHTING:
                hasLighting = true;
                break;
            case SpellType.CHARM:
                hasCharm = true;
                break;
            case SpellType.DOUBLEJUMP:
                doubleJumpEnabled = true;
                break;
        }
    }

    public void BoughtItem(Item item)
    {
        Debug.Log("Bought an item: " + item.name);
    }

    public bool TrySpendCandyAmount(int spendCandyAmount)
    {
        if (candyAmount >= spendCandyAmount)
        {
            GainCandy(-spendCandyAmount);
            return true;
        }
        return false;
    }

    public void AllowMovement(bool active)
    {
        canMove = active;
        if(active)
        {
            characterCollider.material = frictionLessMaterial;
        }
        else
        {
            characterCollider.material = frictionMaterial;
        }
    }

    public void EnterShop()
    {
        HideUI();
        AllowMovement(false);
    }

    public void ExitShop()
    {
        //ShowUI();
        AllowMovement(true);
    }

    public void SpawnSmokePoof()
    {
        EffectsManager.instance.SpawnSmokePoof(smokePoofSpawnPoint);
    }

    public void SpawnCatSpellPoof()
    {
        EffectsManager.instance.SpawnCatSpellPoof(smokePoofSpawnPoint);
    }

    public IEnumerator SuzyCatJump()
    {
        yield return new WaitForSeconds(0.25f);
        SetGrounded(false);
        isJumping = true;
        rb.velocity = (rb.velocity.x * Vector3.right) + Vector3.up * jumpSpeed;
    }
}
