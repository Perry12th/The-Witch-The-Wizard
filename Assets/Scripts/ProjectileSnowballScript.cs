using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSnowballScript : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;

    public float speed;

    private void Start()
    {
        Destroy(gameObject, 10);
        rb.velocity = ((gameObject.transform.up + gameObject.transform.right) * -speed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("FireBall") || collision.gameObject.CompareTag("Pumpkin") || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Pumpkin") || collision.gameObject.CompareTag("TSnowball") || collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject, 0.1f);
        }
    }
}
