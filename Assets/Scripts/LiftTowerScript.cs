using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LiftTowerScript : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Light light;
    [SerializeField]
    private Color greenLight;
    [SerializeField]
    private Color redLight;
    private bool isPowered = false;
    private bool isGoingLeft = true;

    public void OnPowered()
    {
        if (!isPowered)
        {
            light.enabled = true;
            animator.enabled = true;
        }

        isPowered = true;
    }

    public void SwitchLighting()
    {
        isGoingLeft = !isGoingLeft;

        if (isGoingLeft)
        {
            light.color = greenLight;
        } 
        else
        {
            light.color = redLight;
        }
    }

}
