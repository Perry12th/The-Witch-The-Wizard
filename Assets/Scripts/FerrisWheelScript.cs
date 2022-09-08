using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FerrisWheelScript : MonoBehaviour, IElectrical
{
    [SerializeField]
    private Transform wheelTransform;
    [SerializeField]
    private Transform rotationPoint;
    [SerializeField]
    private float rotationSpeed = 5f;
    private bool isPowered;

    public UnityEvent OnPoweredUp;
    private void Update()
    {
        if (isPowered)
        {
            float roationAmount = rotationSpeed * Time.deltaTime;
            wheelTransform.RotateAround(rotationPoint.position, new Vector3(0, 0, 1), roationAmount);
        }
    }

    public void OnPowerDown()
    {
        isPowered = false;
    }

    public void OnPowered()
    {
        isPowered = true;
        OnPoweredUp.Invoke();
    }
}
