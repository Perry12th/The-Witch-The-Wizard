﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxScript : MonoBehaviour
{
    [SerializeField]
    private WitcherScript witcher;
    [SerializeField]
    private float rotationPerUnit = 0.5f;
    [SerializeField]
    private float startingRotation = 0;
    private float lastXPosition;

    [SerializeField]
    private Vector2 parallaxEffectMultiplier;
    [SerializeField]
    private bool infiniteHorizontal;
    [SerializeField]
    private bool infiniteVertical;

    private Transform cameraTransform;
    private Vector3 lastCameraPosition;
    private float textureUnitSizeX;
    private float textureUnitSizeY;
    private void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
        RenderSettings.skybox.SetFloat("_Rotation", startingRotation);
        //Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        //Texture2D texture = sprite.texture;
        //textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        //textureUnitSizeY = texture.height / sprite.pixelsPerUnit;
    }
    private void Update()
    {
        //RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotateSpeed);
        float currentRotation = RenderSettings.skybox.GetFloat("_Rotation");
        RenderSettings.skybox.SetFloat("_Rotation", currentRotation + (witcher.transform.position.x - lastXPosition) * rotationPerUnit);
        lastXPosition = witcher.transform.position.x;
    }

    private void LateUpdate()
    {
        //Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        //transform.position += new Vector3(deltaMovement.x * parallaxEffectMultiplier.x, deltaMovement.y * parallaxEffectMultiplier.y);
        //lastCameraPosition = cameraTransform.position;

        //if (infiniteHorizontal)
        //{
        //    if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX)
        //    {
        //        float offsetPositionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
        //        transform.position = new Vector3(cameraTransform.position.x + offsetPositionX , transform.position.y);
        //    }
        //}

        //if (infiniteVertical)
        //{
        //    if (Mathf.Abs(cameraTransform.position.y - transform.position.y) >= textureUnitSizeY)
        //    {
        //        float offsetPositionY = (cameraTransform.position.y - transform.position.y) % textureUnitSizeY;
        //        transform.position = new Vector3(transform.position.x, cameraTransform.position.y + offsetPositionY);
        //    }
        //}
    }
}
