using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetScript : MonoBehaviour
{
    [SerializeField]
    WitcherScript witcher;

    private void OnTriggerStay(Collider collision)
    {
        Debug.Log("Stay");
        if (collision.gameObject.CompareTag("Ground"))
        {
            witcher.SetGrounded(true);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("Enter");
        if (collision.gameObject.CompareTag("Ground"))
        {
            witcher.SetGrounded(true);
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        Debug.Log("Exit");
        if (collision.gameObject.CompareTag("Ground"))
        {
            witcher.SetGrounded(false);
            witcher.Recover();
        }
    }

}
