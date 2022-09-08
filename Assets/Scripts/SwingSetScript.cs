using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingSetScript : MonoBehaviour
{
    [SerializeField]
    private Transform rotationPivotPoint;
    [SerializeField]
    private Transform shipPivotPoint;
    [SerializeField]
    private SwingShipScript swingShipScript;
    [SerializeField]
    private ElectricalTriggerScript electricalTrigger;
    [SerializeField]
    private List<Light> lights;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float shipRotationSpeed;
    [SerializeField]
    private float rotationLimit = 35.0f;
    [SerializeField]
    private bool powerOnStart = false;
    private bool isPowered;
    private bool goingLeft = true;
    private float totalRotation;

    public void Start()
    {
        electricalTrigger.PowerOn += OnPowered;
        electricalTrigger.PowerOff += OnPowerDown;
        totalRotation = rotationLimit;
        if (powerOnStart)
        {
            OnPowered();
        }
    }

    public void Update()
    {
        if (isPowered)
        {
            rotationPivotPoint.localEulerAngles += new Vector3(0, (goingLeft ? rotationSpeed : -rotationSpeed) * Time.deltaTime, 0);
            shipPivotPoint.localEulerAngles += new Vector3(0, 0, (goingLeft ? shipRotationSpeed : -shipRotationSpeed) * Time.deltaTime);
            totalRotation += rotationSpeed * Time.deltaTime;
            if (totalRotation >= rotationLimit * 2)
            {
                goingLeft = !goingLeft;
                totalRotation = 0;
            }
        }
    }

    public void OnPowerDown()
    {
        if (isPowered)
        {
            swingShipScript.OnPowerDown();
            foreach (Light light in lights)
            {
                light.enabled = false;
            }
            isPowered = false;
        }
        
    }

    public void OnPowered()
    {
        if (!isPowered)
        {
            swingShipScript.OnPowered();
            foreach(Light light in lights)
            {
                light.enabled = true;
            }
            isPowered = true;
        }
        
    }

}
