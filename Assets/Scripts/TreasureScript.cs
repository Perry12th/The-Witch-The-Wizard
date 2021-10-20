using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureScript : MonoBehaviour, IDamagable
{
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private GameObject drop;
    [SerializeField]
    private int life = 2;

    private void OnTriggerEnter(Collider other)
    {
        FireballScript fireball = other.GetComponent<FireballScript>();

        if (fireball != null)
        {
            fireball.ApplyDamage(gameObject);
            fireball.DestroyFireball();
        }
    }

    public void ApplyDamage(int damageTaken = 1)
    {
        life -= damageTaken;
        anim.SetTrigger("Hit");
        if (life <= 0)
        {
            Instantiate(drop, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }
}
