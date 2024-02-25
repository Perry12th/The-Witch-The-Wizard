using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class SnakeScript : MonoBehaviour, IDamagable, ICharmable
{
    [SerializeField]
    private GameObject fireBallPrefab;
    [SerializeField]
    private PlayerSpotter popOutSpotter;
    [SerializeField]
    private PlayerSpotter popInSpotter;
    [SerializeField]
    private new BoxCollider collider;
    [SerializeField]
    private Transform fireballSpawn;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private ParticleSystem charmParticles1;
    [SerializeField]
    private ParticleSystem charmParticles2;
    [SerializeField]
    private float charmTime;
    [SerializeField]
    private float charmEnableTime;
    [SerializeField] 
    private bool facingRight;
    [SerializeField] 
    private float rotationTime;
    private bool isRotating;
    private SugarBallScript activeFireBall;
    private int charmLevel;
    private const int MaxCharm = 3;
    private bool canFire;
    private bool canCharm;
    private bool fullyCharmed;
    private Vector3 lastPlayerPosition;
    private static readonly int In = Animator.StringToHash("PopIn");
    private static readonly int Out = Animator.StringToHash("PopOut");
    private static readonly int LightCharmed = Animator.StringToHash("LightCharmed");
    private static readonly int Charmed = Animator.StringToHash("Charmed");
    private static readonly int HeavyCharmed = Animator.StringToHash("HeavyCharmed");
    private static readonly int Idle = Animator.StringToHash("Idle");
    private static readonly int FireBack = Animator.StringToHash("FireBack");

    public void OnEnable()
    {
        popOutSpotter.playerSpotted += PopOut;
        popInSpotter.playerLeft += PopIn;
        canFire = true;
        canCharm = true;
    }

    public void OnDisable()
    {
        popOutSpotter.playerSpotted -= PopOut;
        popInSpotter.playerLeft -= PopIn;
    }

    public void Update()
    {
        if (!popInSpotter.playerWithinRange || fullyCharmed || isRotating) return;
            
        if (transform.position.x > popInSpotter.player.transform.position.x && facingRight)
        {
            StartCoroutine(RotateToPoint(rotationTime,-90));
        }
        else if (transform.position.x < popInSpotter.player.transform.position.x && !facingRight)
        {
            StartCoroutine(RotateToPoint(rotationTime,90));
        }
    }

    private IEnumerator RotateToPoint(float rotateTime, float rotateTargetY)
    {
        float counter = 0;
        
        Quaternion currentRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, rotateTargetY, 0);

        isRotating = true;
        
        while (counter < rotateTime)
        {
            transform.rotation = Quaternion.Lerp(currentRotation, targetRotation, counter / rotateTime);
            counter += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        transform.rotation = targetRotation;
        
        facingRight = !facingRight;

        isRotating = false;
    }

    private void PopIn()
    {
        Debug.Log("PopIn" + gameObject.name);
        if (popInSpotter.playerWithinRange) return;
        collider.enabled = false;
        animator.SetTrigger(In);
    }

    private void PopOut()
    {
        if (collider.enabled) return;
        collider.enabled = true;
        animator.SetTrigger(Out);
    }

    public void ApplyCharm()
    {
        if (!canCharm || charmLevel >= MaxCharm) return;
        
        charmLevel = Mathf.Clamp(charmLevel + 1, 0, MaxCharm);

        switch (charmLevel)
        {
            case 1:
                animator.SetTrigger(LightCharmed);
                StartCoroutine(CharmTimer());
                break;
            case 2:
                animator.SetTrigger(Charmed);
                StopCoroutine(CharmTimer());
                StartCoroutine(CharmTimer());
                break;
            case 3:
                charmParticles1.Play();
                charmParticles2.Play();
                fullyCharmed = true;
                collider.enabled = false;
                animator.SetTrigger(HeavyCharmed);
                StopCoroutine(CharmTimer());
                StartCoroutine(CharmTimer());
                break;
        }
        canCharm = false;
        StopCoroutine(CharmTimer());
        StartCoroutine(CharmEnabler());
    }

    private void LowerCharm()
    {
        StopAllCoroutines();
        charmLevel = 0;
        collider.enabled = true;
        fullyCharmed = false;
        charmParticles1.Stop();
        animator.SetTrigger(Idle);
        charmParticles2.Stop();
    }

    public void ApplyDamage(int damageTaken = 1)
    {
        if (fullyCharmed)
        {
            EffectsManager.instance.SpawnSmokePoof(transform);
            Destroy(gameObject);
        }
        else if (canFire && charmLevel < 2)
        {
            CheckIfPlayerInRange();
            StopCoroutine(CharmTimer());
            charmLevel = 0;
            animator.SetTrigger(FireBack);
        }
    }

    public void FireFireball()
    {
        canFire = false;
        activeFireBall = Instantiate(fireBallPrefab, fireballSpawn.position, fireBallPrefab.transform.rotation).GetComponent<SugarBallScript>();
        activeFireBall.transform.LookAt(lastPlayerPosition);
        activeFireBall.EnableBallMovement();
    }

    public void Recover()
    {
        animator.SetTrigger(Idle);
        canFire = true;
    }

    private void CheckIfPlayerInRange()
    {
        if (popInSpotter.playerWithinRange)
        {
            lastPlayerPosition = popInSpotter.player.transform.position;
        }
    }

    private IEnumerator CharmEnabler()
    {
        yield return new WaitForSeconds(charmEnableTime);
        canCharm = true;
    }

    private IEnumerator CharmTimer()
    {
        yield return new WaitForSeconds(charmTime);
        LowerCharm();
    }
}
