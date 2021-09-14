using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceTriggerScript : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TurnSnowman();
        }
    }

    public void TurnSnowman()
    {

        animator.SetTrigger("FlipUp");
    }
}
