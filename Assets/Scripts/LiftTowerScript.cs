using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LiftTowerScript : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Light towerLight;
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
            towerLight.enabled = true;
            animator.enabled = true;
        }

        isPowered = true;
    }

    public void OnPowerDown()
    {
        if (isPowered)
        {
            towerLight.enabled = false;
            animator.enabled = false;
        }

        isPowered = false;
    }

    public void SwitchLighting(bool goingLeft)
    {
        isGoingLeft = goingLeft;

        if (isGoingLeft)
        {
            towerLight.color = greenLight;
        } 
        else
        {
            towerLight.color = redLight;
        }
    }

}
