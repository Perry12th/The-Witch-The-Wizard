using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMagicScript : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 10);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<WitcherScript>().ApplyDamage();
        }
        Destroy(gameObject);
    }

    public void SetDirection(Vector3 direction)
    {
        rb.velocity = (direction * speed);
    }
}
