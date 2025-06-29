﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyChairLiftScript : MonoBehaviour, IElectrical
{
    [SerializeField]
    private float zCorrection = -8;
    [SerializeField]
    private GameObject candyCaneParent;
    [SerializeField]
    private PathCreation.Examples.PathFollower pathFollower;
    [SerializeField]
    private bool isGoingLeft = true;
    [SerializeField]
    private LiftRoofScript liftRoofScript;
    private WitcherScript witcher;

    public void Update()
    {
        if (witcher != null)
        {
            if (witcher.GetIsLookingRight())
            {
                witcher.GetPlayerModel().transform.eulerAngles = new Vector3(0, 90.0f, 0);
            }
            else
            {
                witcher.GetPlayerModel().transform.eulerAngles = new Vector3(0, 270.0f, 0);
            }
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        WitcherScript witcher = collision.gameObject.GetComponent<WitcherScript>();
        if (witcher != null)
        {
            this.witcher = witcher;
            this.witcher.transform.parent = transform;
            this.witcher.GetRigidBody().useGravity = false;
        }
    }

    public void OnCollisionExit(Collision collision)
    {

            if (witcher != null && collision.gameObject == witcher.gameObject)
            {
                witcher.transform.parent = null;
                witcher.GetRigidBody().useGravity = true;
                //player.canMove = true;
                if (witcher.GetIsLookingRight())
                {
                    witcher.GetPlayerModel().transform.eulerAngles = new Vector3(0, 90.0f, 0);
                }
                else
                {
                    witcher.GetPlayerModel().transform.eulerAngles = new Vector3(0, 270.0f, 0);
                }
                witcher.transform.position = new Vector3(witcher.transform.position.x, witcher.transform.position.y, zCorrection);
                witcher = null;
            }
            

    }

    public void FlipChair(bool goingLeft)
    {

        isGoingLeft = goingLeft;
        pathFollower.SetIsGoingLeft(goingLeft);
        float pathSpeed = pathFollower.GetSpeed();
        pathFollower.SetSpeed(goingLeft? Mathf.Abs(pathSpeed) : -Mathf.Abs(pathSpeed));
        if (isGoingLeft)
        {
            candyCaneParent.transform.localEulerAngles = new Vector3(candyCaneParent.transform.localEulerAngles.x, 270.0f, candyCaneParent.transform.localEulerAngles.z);
        }
        else if (!isGoingLeft)
        {
            candyCaneParent.transform.localEulerAngles = new Vector3(candyCaneParent.transform.localEulerAngles.x, 90.0f, candyCaneParent.transform.localEulerAngles.z);
        }
    }

    public PathCreation.Examples.PathFollower GetPathFollower()
    {
        return pathFollower;
    }

    public void OnPowered()
    {
        liftRoofScript.onLiftTowerPowered.Invoke();
    }

    public void OnPowerDown()
    {
        pathFollower.enabled = false;
    }
}
