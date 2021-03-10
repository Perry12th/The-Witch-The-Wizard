using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinScript : MonoBehaviour
{
    [SerializeField]
    GameObject path;

    [SerializeField]
    SpriteRenderer sr;

    public bool isWalkable;
    public float speed;
    public bool flipping = false;

    private void Start()
    {
        path.transform.parent = null;
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
            Destroy(gameObject);
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
}
