using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftDropPointScript : MonoBehaviour
{
    [SerializeField]
    LiftScript liftScript;
    [SerializeField]
    private bool isLeftDropPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (liftScript.isPowered && (isLeftDropPoint == liftScript.isGoingLeft))
            {
                liftScript.PowerOff();
            }
        }
    }
}
