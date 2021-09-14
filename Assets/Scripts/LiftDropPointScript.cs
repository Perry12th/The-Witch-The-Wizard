using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftDropPointScript : MonoBehaviour
{
    [SerializeField]
    private LiftScript liftScript;
    [SerializeField]
    private bool isLeftDropPoint;
    [SerializeField]
    private Vector3 defaultChairLiftRotation;

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

    private void OnTriggerStay(Collider other)
    {
        CandyChairLiftScript candyChairLift = other.gameObject.GetComponent<CandyChairLiftScript>();
        if (candyChairLift != null && liftScript.isPowered)
        {
            if (isLeftDropPoint == liftScript.isGoingLeft)
            {
                candyChairLift.GetPathFollower().SetUsingPathRotation(true);
            }
            else
            {
                candyChairLift.GetPathFollower().SetUsingPathRotation(false);
                candyChairLift.GetPathFollower().transform.eulerAngles = new Vector3(defaultChairLiftRotation.x, candyChairLift.GetPathFollower().transform.eulerAngles.y, defaultChairLiftRotation.z);
            }
        }
    }
}
