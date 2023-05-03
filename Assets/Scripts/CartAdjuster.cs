using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartAdjuster : MonoBehaviour
{
    [SerializeField] Cart cart;
    [SerializeField] private float newSpeed;
    [SerializeField] private float time;
    
        private void OnTriggerEnter(Collider collision)
        {
            if (collision?.gameObject.GetComponent<Cart>() != null)
            {
                cart.AdjustSpeed(newSpeed,time);
            }
        }
}
