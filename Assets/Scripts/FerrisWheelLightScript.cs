using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FerrisWheelLightScript : MonoBehaviour
{
    [SerializeField]
    private FerrisWheelScript ferrisWheelScript;
    [SerializeField]
    private Light wheelLight;

    private void Start()
    {
        wheelLight.enabled = false;
        ferrisWheelScript.OnPoweredUp.AddListener(OnPowered);
    }



    private void OnPowered()
    {
        wheelLight.enabled = true;
    }
}
