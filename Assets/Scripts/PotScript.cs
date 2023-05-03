using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotScript : MonoBehaviour, IDamagable
{
    public void ApplyDamage(int damageTaken = 1)
    {
        EffectsManager.instance.SpawnSmokePoof(transform);
        Destroy(gameObject);
    }
}
