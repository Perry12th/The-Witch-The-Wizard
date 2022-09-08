using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinemachineScript : MonoBehaviour
{
    [SerializeField]
    private PlayerSpotter playerSpotter;
    [SerializeField]
    private CameraScript cameraScript;
    [SerializeField]
    private Vector3 cameraOffset;

    private void Start()
    {
        playerSpotter.playerSpotted += PlayerSpotted;
        playerSpotter.playerLeft += PlayerLeft;
    }

    private void PlayerLeft()
    {
        cameraScript.ResetCamera(false);
    }

    private void PlayerSpotted()
    {
        cameraScript.ChangeCameraOffset(cameraOffset);
    }
}
