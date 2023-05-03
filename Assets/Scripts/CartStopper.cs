using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartStopper : MonoBehaviour
{
    [SerializeField] Cart cart;
    [SerializeField] private Rigidbody bumperRigidbody;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.GetComponent<Cart>() == null) return;
        bumperRigidbody.constraints = RigidbodyConstraints.None;
        cart.LeaveCart();
    }
}
