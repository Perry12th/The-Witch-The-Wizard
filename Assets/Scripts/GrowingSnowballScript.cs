using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingSnowballScript : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;

    public float speed;

    private void Start()
    {
        Destroy(gameObject, 6);
        rb.velocity = ((gameObject.transform.up + gameObject.transform.right) * -speed);
    }

    private void Update()
    {
        if(rb.velocity.magnitude > 0.5f)
        {
            if (transform.localScale.x < 6)
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z) + new Vector3(0.008f, 0.008f, 0);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("FireBall") || collision.gameObject.CompareTag("Pumpkin") || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("TSnowball") || collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
