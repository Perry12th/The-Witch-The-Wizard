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

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = ((gameObject.transform.right) * speed);
        Destroy(gameObject, 10);
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
        Destroy(particle, 1);
        Destroy(gameObject);
    }
}
