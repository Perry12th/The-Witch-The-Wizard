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
        rb.velocity = ((gameObject.transform.up + gameObject.transform.right) * speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Lava"))
        {
            Instantiate(icePlat, transform.position, icePlat.transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
