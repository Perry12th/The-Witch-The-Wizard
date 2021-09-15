using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowballScript : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private GameObject icePlat;

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
            var rotation = Quaternion.LookRotation(collision.GetContact(0).normal);
            GameObject icePlatform = Instantiate(icePlat, collision.GetContact(0).point, rotation);
            icePlatform.transform.Rotate(270, icePlatform.transform.rotation.y, icePlatform.transform.rotation.z);
        }
        Destroy(gameObject);
    }
}
