using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SugarPlumScript : MonoBehaviour, IDamagable
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private ParticleSystem sugarParticle;
    [SerializeField]
    private Collider sugarCollider;
    [SerializeField]
    private GameObject sugarBallPrefab;
    [SerializeField]
    private Transform sugarBallSpawnPoint;
    [SerializeField]
    private PlayerSpotter playerSpotter;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float distanceCutOff;
    [SerializeField]
    private List<Transform> partolPoints;
    [SerializeField]
    private Transform sugarBallLeftHolder;
    [SerializeField]
    private Transform sugarBallRightHolder;
    [SerializeField]
    private int life;
    [SerializeField]
    private float fireTimer;
    [SerializeField]
    private float idleTimer = 2.0f;
    private float timer;
    private int currentPartolPoint;
    private Vector3 directionVector;
    private Vector3 lastPlayerPosition;
    private Transform activeSugarBallHolder;
    private SugarBallScript activeSugarBall;

    private SugarPlumStates plumState = SugarPlumStates.FLYING;

    private enum SugarPlumStates
    {
        FLYING,
        IDLE,
        FLIPPING,
        FIRING,
        HURTING,
        DYING
    }

    private void Update()
    {
        timer += Time.deltaTime;
        switch (plumState)
        {
            case SugarPlumStates.FLYING:
                if (Vector3.Distance(transform.position, partolPoints[currentPartolPoint].transform.position) <= distanceCutOff)
                {
                    HeadToNextPatrolPoint();
                }
                transform.position += (directionVector * speed * Time.deltaTime);
                CheckIfPlayerInRange();
                break;
            case SugarPlumStates.IDLE:
                {
                    if (timer > idleTimer)
                    {
                        timer = 0;
                        plumState = SugarPlumStates.FLYING;
                    }
                }
                break;
        }
    }

    private void CheckIfPlayerInRange()
    {
        if (playerSpotter.playerWithinRange && timer >= fireTimer)
        {
           timer = 0;
           plumState = SugarPlumStates.FIRING;
           lastPlayerPosition = playerSpotter.player.transform.position;
           BeginAttackAnimation();
        }
    }

    private void HeadToNextPatrolPoint()
    {
        currentPartolPoint++;

        if (currentPartolPoint > partolPoints.Count - 1)
        {
            currentPartolPoint = 0;
        }

        Vector3 lastDirection = directionVector;
        directionVector = (partolPoints[currentPartolPoint].position - transform.position);
        directionVector.Normalize(); 
    }

    private void BeginAttackAnimation()
    {
        float rad = Random.Range(0.0f, 1.0f);
        if (rad < 0.5)
        {
            animator.SetTrigger("AttackLeft");
            activeSugarBallHolder = sugarBallLeftHolder;
        }
        else
        {
            animator.SetTrigger("AttackRight");
            activeSugarBallHolder = sugarBallRightHolder;
        }

        activeSugarBall = Instantiate(sugarBallPrefab, activeSugarBallHolder.position, sugarBallPrefab.transform.rotation).GetComponent<SugarBallScript>();

    }

    public void FireSugarPlumBall()
    {
        activeSugarBall?.transform.LookAt(lastPlayerPosition);
        activeSugarBall?.EnableBallMovement();
        activeSugarBall = null;
    }

    public void ApplyDamage(int damageTaken = 1)
    {
        if (plumState != SugarPlumStates.DYING)
        {
            life -= damageTaken;
            if (life <= 0)
            {
                sugarParticle.Stop(true);
                sugarCollider.enabled = false;
                plumState = SugarPlumStates.DYING;
                animator.SetTrigger("Death");
                if (activeSugarBall != null)
                {
                    Destroy(activeSugarBall);
                }
            }
            else
            {
                plumState = SugarPlumStates.HURTING;
                animator.SetTrigger("Attacked");
            }
        }
    }

    public void Recover()
    {
        plumState = SugarPlumStates.FLYING;
        animator.SetTrigger("Idle");   
    }

    public void RecoverFromFiring()
    {
        plumState = SugarPlumStates.IDLE;
        animator.SetTrigger("Idle");
    }

    public void SpawnSmokePoof()
    {
        EffectsManager.instance.SpawnSmokePoof(transform);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }

}
