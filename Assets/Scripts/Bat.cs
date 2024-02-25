using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour, IDamagable
{
    [SerializeField] private PlayerSpotter firstSpotter;
    [SerializeField] private PlayerSpotter secondSpotter;
    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private float flySpeed;
    private bool flying;
    private void Start()
    {
        firstSpotter.playerSpotted += FlyAway;
        secondSpotter.playerLeft += Disappear;
    }

    private void Disappear()
    {
        if (flying)
        {
            Destroy(gameObject);
        }
    }

    private void FlyAway()
    {
        Debug.Log("FlyAway");
        animator.SetTrigger("Spotted");
        flying = true;
    }

    private void Update()
    {
        if (flying)
        {
            transform.position += transform.forward * flySpeed * Time.deltaTime;
        }
    }

    public void ApplyDamage(int damageTaken = 1)
    {
        EffectsManager.instance.SpawnSmokePoof(transform); 
        Destroy(gameObject);
    }
}
