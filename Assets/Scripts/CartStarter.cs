using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartStarter : MonoBehaviour, IElectrical
{
    [SerializeField] Cart cart;
    [SerializeField] private Collider triggerCollider;
    public void OnPowerDown()
    {
    }

    public void OnPowered()
    {
        cart.EnableEffects();
        triggerCollider.enabled = false;
    }
}
