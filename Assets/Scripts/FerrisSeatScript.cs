﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerrisSeatScript : MonoBehaviour, IElectrical
{
    [SerializeField]
    private FerrisWheelScript ferrisWheelScript;
    [SerializeField]
    private Transform basketFollowTransform;
    //[SerializeField]
    //private float zCorrection = -8;
    private WitcherScript witcher;
    private bool isPowered;

    private void Start()
    {
        ferrisWheelScript?.OnPoweredUp.AddListener(OnPowerUp);
    }
    private void LateUpdate()
    {
        if (isPowered)
        {
            transform.position = basketFollowTransform.position;
        }     
    }

    private void OnCollisionEnter(Collision collision)
    {
        WitcherScript witcher = collision.gameObject.GetComponent<WitcherScript>();
        if (witcher != null)
        {
            this.witcher = witcher;
            this.witcher.transform.parent = transform;
            this.witcher.GetRigidBody().useGravity = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (witcher != null)
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
            //witcher.transform.position = new Vector3(witcher.transform.position.x, witcher.transform.position.y, zCorrection);
            witcher = null;
        }
    }

    public void OnPowerDown()
    {
        isPowered = false;
    }

    public void OnPowered()
    {
        if (!isPowered)
        {
            ferrisWheelScript.OnPowered();
        }
    }

    private void OnPowerUp()
    {
        isPowered = true;
    }
}