using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucky : MonoBehaviour, IDamagable
{
    [Header("Balloon")] 
    [SerializeField] List<GameObject> balloons;
    [SerializeField] GameObject balloonPopEffect;
    [SerializeField] Transform balloonRespawnPoofPoint;
    [SerializeField] GameObject balloonRespawnPoof;

    [Header("Animation")] 
    [SerializeField] Animator animator;

    [SerializeField] private float maxLeftAngle;
    [SerializeField] private float maxRightAngle;

    [Header("Movement")] 
    [SerializeField] int flightSpeed;
    [SerializeField] private Rigidbody rigidbody;

    [Header("Attack")] 
    [SerializeField] GameObject magicalAttack;
    [SerializeField] int attackCooldown;
    [SerializeField] bool attackReady;

    [Header("Status")] 
    [SerializeField] ClownState clownState;
    int health;
    [SerializeField] int maxHealth;

    [Header("Other")] 
    [SerializeField] private PlayerSpotter playerSpotter;
    [SerializeField] private Collider luckyCollider;
    [SerializeField] private float deathTimer;
    [SerializeField] private float floatingTimer;

    private void Start()
    {
        health = maxHealth;
    }

    private void Update()
    {
        if (clownState == ClownState.IDLE && playerSpotter.playerWithinRange && !attackReady)
        {
            PreformMovement();
        }
    }
    

    public void ApplyDamage(int damageTaken = 1)
    {
        if (clownState == ClownState.IDLE)
        {
            health--;
            PopBalloon(health);
        }
    }

    public void Recover()
    {
        SetClownState(ClownState.IDLE);
    }

    private void PopBalloon(int currentHeath)
    {
        Debug.Log(currentHeath);
        GameObject balloon = balloons[currentHeath];
        Instantiate(balloonPopEffect, balloon.transform);
        balloon.SetActive(false);
        if (currentHeath != 0)
        {
            SetClownState(ClownState.ATTACKED);
        }
        else
        {
            SetClownState(ClownState.FALLING);
            rigidbody.useGravity = true;
            rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX |
                                    RigidbodyConstraints.FreezePositionZ;
        }
    }

    public void Rebound()
    {
        health = maxHealth;
        SpawnBalloonPoof();
        foreach(GameObject balloon in balloons)
        {
            balloon.SetActive(true);
        }
        rigidbody.useGravity = false;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
        Recover();
    }

    private void SpawnBalloonPoof()
    {
        EffectsManager.instance.SpawnSmokePoof(balloonRespawnPoof.transform);
    }

    private void SetClownState(ClownState clownState)
    {
        this.clownState = clownState;
        animator.SetTrigger(clownState.ToString());
    }
    
    private void PreformMovement()
    {
        Transform playerTransform = playerSpotter.player.transform;

        float distance = Vector2.Distance(transform.position, playerTransform.position + new Vector3(0, 4,0));
        Vector2 direction = playerTransform.position - transform.position;
        direction.Normalize();
        if (distance > 2)
        {
            transform.position =
                Vector2.MoveTowards(transform.position, playerTransform.position + new Vector3(0, 7,0), flightSpeed * Time.deltaTime);
            Vector3 lookAtTransform = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
            transform.LookAt(lookAtTransform);

        }
    }
    private void PreformAttack()
    {
        SetClownState(ClownState.ATTACKING);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Lava" && clownState == ClownState.FALLING)
        {
            SetClownState(ClownState.DEATH);
            DeathTimer();
        }
        else if (collision.gameObject.tag == "Ground" && clownState == ClownState.FALLING)
        {
            SetClownState(ClownState.REBOUNDING);
        }
    }

    private void DeathTimer()
    {
        StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        luckyCollider.enabled = false;
        yield return new WaitForSeconds(floatingTimer);
        rigidbody.useGravity = false;
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
        yield return new WaitForSeconds(deathTimer);
        EffectsManager.instance.SpawnSmokePoof(transform);
        Destroy(gameObject);
    }
}

public enum ClownState
{ 
    PAUSED,
    IDLE,
    ATTACKED,
    ATTACKING,
    FALLING,
    REBOUNDING,
    DEATH
}

