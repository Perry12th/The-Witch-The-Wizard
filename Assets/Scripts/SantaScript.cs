using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantaScript : MonoBehaviour
{
    [SerializeField]
    GameObject snowball;

    [SerializeField]
    Transform projPosition, projPosition2;

    [SerializeField]
    GameObject path;

    [SerializeField]
    SpriteRenderer sr;

    public bool isWalkable;
    public float speed;
    public bool flipping = false;

    public int life;

    private void Start()
    {
        path.transform.parent = null;
        InvokeRepeating("SpawnSnowball", 0, 3.5f);
        life = 4;
    }

    private void Update()
    {
        if (isWalkable)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
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

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Path") && isWalkable && !flipping)
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        flipping = true;
        speed *= -1;
        sr.flipX = !sr.flipX;
        Invoke("ClearFlip", 0.1f);
    }

    public void ClearFlip()
    {
        flipping = false;
    }

    public void SpawnSnowball()
    {
        if (speed < 0)
        {
            GameObject ball = Instantiate(snowball, projPosition2.position, projPosition2.rotation);
            ball.transform.Rotate(Vector3.up, 180);
        }
        else
        {
            GameObject ball = Instantiate(snowball, projPosition.position, projPosition.rotation);
        }
    }
}
