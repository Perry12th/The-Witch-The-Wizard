using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowingSnowballScript : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float growSpeedThershold = 0.5f;
    [SerializeField]
    private float growSpeed= 0.01f;
    [SerializeField]
    private float maxSize = 6.0f;

    public void Start()
    {
        Destroy(gameObject, 6);
        rb.velocity = ((gameObject.transform.up + gameObject.transform.right) * -speed);
    }

    public void Update()
    {
        if(rb.velocity.magnitude > growSpeedThershold)
        {
            if (transform.localScale.x < maxSize)
            {
                transform.localScale += (Vector3.one * growSpeed);
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

    public void SetMaxSize(float newMaxSize)
    {
        maxSize = newMaxSize;

        if (transform.localScale.x > maxSize)
        {
            Debug.Log("Resize");
            transform.localScale = new Vector3(maxSize, maxSize, maxSize);
        }
    }
}

