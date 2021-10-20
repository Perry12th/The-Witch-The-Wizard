using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxScript : MonoBehaviour
{
    [SerializeField]
    private WitcherScript witcher;
    [SerializeField]
    private float rotationPerUnit = 0.5f;
    private float lastXPosition;

    private void Update()
    {
        //RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotateSpeed);
        float currentRotation = RenderSettings.skybox.GetFloat("_Rotation");
        RenderSettings.skybox.SetFloat("_Rotation", currentRotation + (witcher.transform.position.x - lastXPosition) * rotationPerUnit);
        lastXPosition = witcher.transform.position.x;
    }
}
