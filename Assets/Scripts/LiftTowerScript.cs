using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LiftTowerScript : MonoBehaviour
{
    public Light light;
    private bool isPowered = false;

    public void OnPowered()
    {
        if (!isPowered)
        {
            light.enabled = true;
        }

        isPowered = true;
    }

}
