using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CandyChairLiftScript : MonoBehaviour, IElectrical
{
    public UnityEvent onLiftTowerPowered;
    private bool isPowered = false;

    public PathCreation.Examples.PathFollower pathFollower;
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = transform;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = null;
        }
    }

    public void OnPowerDown()
    {
    }

    public void OnPowered()
    {
        if (!isPowered)
        {
            onLiftTowerPowered?.Invoke();
        }

        isPowered = true;
    }
}
