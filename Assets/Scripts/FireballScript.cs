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
    private ParticleSystem particleSystem;
    [SerializeField]
    private GameObject particle;

    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = ((gameObject.transform.right) * speed);
        Destroy(gameObject, 10);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log(collision.gameObject.name);
            //Debug.Log(collision.transform.position);
            particleSystem.Stop();
            particle.transform.parent = null;
            Destroy(particle, 1);
            Destroy(gameObject);
        }
    }
}
