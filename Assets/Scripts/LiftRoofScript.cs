using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LiftRoofScript : MonoBehaviour, IElectrical
{
    public UnityEvent onLiftTowerPowered;
    public void OnPowerDown()
    {
    }

    public void OnPowered()
    {
        onLiftTowerPowered?.Invoke();
    }
}
