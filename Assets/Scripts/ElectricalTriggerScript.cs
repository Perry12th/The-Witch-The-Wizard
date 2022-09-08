using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricalTriggerScript : MonoBehaviour, IElectrical
{
    public delegate void OnPoweredUp();
    public event OnPoweredUp PowerOn;

    public delegate void OnPoweredDown();
    public event OnPoweredDown PowerOff;

    private bool isPowered = false;
    public void OnPowerDown()
    {
        if (isPowered)
        {
            PowerOff?.Invoke();
            isPowered = false;
        }
    }

    public void OnPowered()
    {
        if (!isPowered)
        {
            PowerOn?.Invoke();
            isPowered = true;
        }
    }

}
