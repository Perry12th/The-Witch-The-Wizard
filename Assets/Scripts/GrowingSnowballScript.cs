using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingSnowballScript : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    private float speed;

    public void Start()
    {
        Destroy(gameObject, 6);
        rb.velocity = ((gameObject.transform.up + gameObject.transform.right) * -speed);
    }

    public void Update()
    {
        if(rb.velocity.magnitude > 0.5f)
        {
            if (transform.localScale.x < 6)
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z) + new Vector3(0.008f, 0.008f, 0);
            }
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("FireBall") || collision.gameObject.CompareTag("Pumpkin") || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("TSnowball") || collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
        rb.velocity = ((gameObject.transform.up + gameObject.transform.right) * -speed);
    }
}

