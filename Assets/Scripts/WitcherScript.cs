using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitcherScript : MonoBehaviour
{
    public Rigidbody rb;
    public SpriteRenderer sr;
    public Material matr;
    public Material matl;
    public GameObject snowball;
    public GameObject fireball;
    public Transform spawnPoint;
    public Transform spawnPointLeft;

    public float speed;
    public float jumpspeed;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2.5f;

    public bool lookingRight = true;
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            if(lookingRight)
            {
                lookingRight = false;
                FlipLeft();
            }
            rb.velocity = new Vector3(-speed, rb.velocity.y, 0);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (!lookingRight)
            {
                lookingRight = true;
                FlipRight();
            }
            rb.velocity = new Vector3(speed, rb.velocity.y, 0);
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!lookingRight)
            {
                GameObject newBall = Instantiate(snowball, Vector3.up * spawnPointLeft.position.y + Vector3.forward * snowball.transform.position.z + Vector3.right * spawnPointLeft.position.x, gameObject.transform.rotation);
                newBall.transform.Rotate(Vector3.up, 180);
            }
            else
            {
                GameObject newBall = Instantiate(snowball, Vector3.up * spawnPoint.position.y + Vector3.forward * snowball.transform.position.z + Vector3.right * spawnPoint.position.x, gameObject.transform.rotation);
            }
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!lookingRight)
            {
                GameObject newBall = Instantiate(fireball, Vector3.up * spawnPointLeft.position.y + Vector3.forward * fireball.transform.position.z + Vector3.right * spawnPointLeft.position.x, gameObject.transform.rotation);
                newBall.transform.Rotate(Vector3.up, 180);
            }
            else
            {
                GameObject newBall = Instantiate(fireball, Vector3.up * spawnPoint.position.y + Vector3.forward * fireball.transform.position.z + Vector3.right * spawnPoint.position.x, gameObject.transform.rotation);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = (rb.velocity.x * Vector3.right) + Vector3.up * jumpspeed;
        }

        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector3.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.velocity += Vector3.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

    }

    public void FlipLeft()
    {
        sr.flipX = true;
    }

    public void FlipRight()
    {
        sr.flipX = false;
    }

    public void ResetPosition()
    {

    }
}
