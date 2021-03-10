using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballScript : MonoBehaviour
{
    public float speed;
    public Rigidbody rb;

    public GameObject icePlat;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10);
        rb.velocity = (gameObject.transform.right * speed) + (gameObject.transform.up * (speed/2));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lava"))
        {
            Instantiate(icePlat, transform.position, icePlat.transform.rotation);
            //Instantiate(icePlat, transform.position, other.transform.rotation);
            Destroy(gameObject);
        }
    }
}
