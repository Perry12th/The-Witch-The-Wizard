using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMagicScript : MonoBehaviour
{
    public float speed;
    public Rigidbody rb;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyPlatform());
        //rb.velocity = (gameObject.transform.right * speed) + (gameObject.transform.up * (speed / 2));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<WitcherScript>().ResetPosition();
        }
    }
    public IEnumerator DestroyPlatform()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
}
