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

    [SerializeField] private Material powerdMaterial;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        wheelLight.enabled = false;
        meshRenderer = GetComponent<MeshRenderer>();
        ferrisWheelScript.OnPoweredUp.AddListener(OnPowered);
    }



    private void OnPowered()
    {
        wheelLight.enabled = true;
        meshRenderer.material = powerdMaterial;
    }
}
