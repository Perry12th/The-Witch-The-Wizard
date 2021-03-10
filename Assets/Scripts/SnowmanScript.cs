using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowmanScript : MonoBehaviour
{
    [SerializeField]
    GameObject snowball;

    [SerializeField]
    Transform projPosition;

    public int life;

    private void Start()
    {
        InvokeRepeating("SpawnSnowball", 0, 6);
        life = 2;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("FireBall"))
        {
            Destroy(collision.gameObject);
            life--;

            if (life <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SpawnSnowball()
    {
        Instantiate(snowball, projPosition.position, projPosition.rotation);
    }

}
