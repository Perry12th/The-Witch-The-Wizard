using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballScript : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private Vector3 rotateSpeed;
    [SerializeField]
    private int damage;
    [SerializeField]
    private ParticleSystem particleEffects;
    [SerializeField]
    private GameObject particle;
    [SerializeField]
    private float lifeTime = 10;
    [SerializeField]
    private float particleImpactLifetime = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = ((gameObject.transform.right) * speed);
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        transform.Rotate(rotateSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        ApplyDamage(collision.gameObject);
        DestroyFireball();

    }

    public void ApplyDamage(GameObject gameObject)
    {
        IDamagable damagable = gameObject.GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.ApplyDamage(damage);
        }

    }
    public void DestroyFireball()
    {
        EffectsManager.instance.SpawnFireBlast(transform);
        particleEffects.Stop();
        particle.transform.parent = null;
        Destroy(particle, particleImpactLifetime);
        Destroy(gameObject);
    }
}
