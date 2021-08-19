using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMagicScript : MonoBehaviour
{
    public float speed;
    public Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<WitcherScript>().PlayerDeath();
        }
        Destroy(gameObject);
    }

    public void SetDirection(Vector3 direction)
    {
        rb.velocity = (direction * speed);
    }
}
