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

    public void OnPowerDown()
    {
        if (isPowered)
        {
            light.enabled = false;
            animator.enabled = false;
        }

        isPowered = false;
    }

    public void SwitchLighting(bool goingLeft)
    {
        isGoingLeft = goingLeft;

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
