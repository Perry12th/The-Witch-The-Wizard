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
        if (collision.gameObject.CompareTag("Lava"))
        {
            Debug.Log(transform.position);
            Debug.Log(collision.GetContact(0).point);
            var rotation = Quaternion.LookRotation(collision.GetContact(0).normal);
            GameObject icePlatform = Instantiate(icePlat, collision.GetContact(0).point, rotation);
            icePlatform.transform.Rotate(270, icePlatform.transform.rotation.y, icePlatform.transform.rotation.z);
            //Instantiate(icePlat, transform.position, other.transform.rotation);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
