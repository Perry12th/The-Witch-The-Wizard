using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeScript : MonoBehaviour, IDamagable, ICharmable
{
    [SerializeField]
    private GameObject fireBallPrefab;
    [SerializeField]
    private PlayerSpotter popOutSpotter;
    [SerializeField]
    private PlayerSpotter popInSpotter;
    [SerializeField]
    private BoxCollider collider;
    [SerializeField]
    private Transform fireballSpawn;
    [SerializeField]
    private Transform targetTransform;
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
    private SugarBallScript activeFireBall;
    private int charmLevel;
    private const int maxCharm = 3;
    private bool canFire;
    private bool canCharm;
    private bool fullyCharmed;
    private Vector3 lastPlayerPosition;

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


    private void PopIn()
    {
        if (!popInSpotter.playerWithinRange)
        {
            collider.enabled = false;
            animator.SetTrigger("PopIn");
        }
    }

    private void PopOut()
    {
        if (!collider.enabled)
        {
            collider.enabled = true;
            animator.SetTrigger("PopOut");
        }
    }

    public void ApplyCharm()
    {
        if (canCharm && charmLevel < maxCharm)
        {
            charmLevel = Mathf.Clamp(charmLevel + 1, 0, maxCharm);

            switch (charmLevel)
            {
                case 1:
                    animator.SetTrigger("LightCharmed");
                    StartCoroutine(CharmTimer());
                    break;
                case 2:
                    animator.SetTrigger("Charmed");
                    StopCoroutine(CharmTimer());
                    StartCoroutine(CharmTimer());
                    break;
                case 3:
                    charmParticles1.Play();
                    charmParticles2.Play();
                    fullyCharmed = true;
                    animator.SetTrigger("HeavyCharmed");
                    StopCoroutine(CharmTimer());
                    StartCoroutine(CharmTimer());
                    break;
            }
            canCharm = false;
            StartCoroutine(CharmEnabler());
        }
    }

    public void LowerCharm()
    {
        charmLevel = 0;
        fullyCharmed = false;
        charmParticles1.Stop();
        animator.SetTrigger("Idle");
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
            animator.SetTrigger("FireBack");
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
        animator.SetTrigger("Idle");
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
