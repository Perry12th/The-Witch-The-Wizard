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
        rb.velocity = (gameObject.transform.right * speed) + (gameObject.transform.up * (speed / 2));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<WitcherScript>().ResetPosition();
        }
    }
}
