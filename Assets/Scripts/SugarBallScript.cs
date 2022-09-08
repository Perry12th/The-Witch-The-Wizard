using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SugarBallScript : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private int damage;
    [SerializeField]
    private ParticleSystem particleEffects;
    [SerializeField]
    private Collider ballCollider;
    [SerializeField]
    private float lifeTime = 10;
    [SerializeField]
    private float particleImpactLifetime = 1;
    [SerializeField]
    private float colliderActviteTime = 0.3f;

    // Start is called before the first frame update
    private void Start()
    {
        ballCollider.enabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        ApplyDamage(collision.gameObject);
        DestroySugarball();

    }
    private void ApplyDamage(GameObject gameObject)
    {
        IDamagable damagable = gameObject.GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.ApplyDamage(damage);
        }

    }

    private void DestroySugarball()
    {
        EffectsManager.instance.SpawnFireBlast(transform);
        particleEffects.Stop();
        particleEffects.transform.parent = null;
        Destroy(particleEffects, particleImpactLifetime);
        Destroy(gameObject);
    }

    public void EnableBallMovement()
    {
        rb.velocity = ((gameObject.transform.forward) * speed);
        Destroy(gameObject, lifeTime);
        StartCoroutine(ActviteCollider());
    }

    IEnumerator ActviteCollider()
    {
        yield return new WaitForSeconds(colliderActviteTime);
        {
            ballCollider.enabled = true;
        }
    }
}
