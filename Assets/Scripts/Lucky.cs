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

    [Header("Movement")]
    [SerializeField] int flightSpeed;

    [Header("Attack")]
    [SerializeField] GameObject magicalAttack;
    [SerializeField] int attackCooldown;
    [SerializeField] bool attackReady;

    [Header("Status")]
    [SerializeField] ClownState clownState;
    int health;
    [SerializeField] int maxHealth;

    
    public void ApplyDamage(int damageTaken = 1)
    {
        health--;
        PopBalloon(health);
    }

    private void PopBalloon(int currentHeath)
    {
        GameObject balloon = balloons[currentHeath];
        Instantiate(balloonPopEffect, balloon.transform);
        balloon.SetActive(false);
    }

    private void Rebound()
    {
        health = maxHealth;
        SpawnBalloonPoof();
        foreach(GameObject balloon in balloons)
        {
            balloon.SetActive(true);
        }
    }

    private void SpawnBalloonPoof()
    {
        Instantiate(balloonRespawnPoof, balloonRespawnPoofPoint);
    }

    private void SetClownState(ClownState clownState)
    {
        this.clownState = clownState;
        animator.SetTrigger(clownState.ToString());
    }
}

public enum ClownState
{ 
    IDLE,
    ATTACKED,
    ATTACKING,
    FALLING,
    REBOUNDING,
    DEATH
}

