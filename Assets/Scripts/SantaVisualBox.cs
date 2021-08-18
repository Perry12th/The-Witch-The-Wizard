using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantaVisualBox : MonoBehaviour
{
    [SerializeField]
    SantaScript santa;
    [SerializeField]
    PumpkinScript pumpkin;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (santa != null)
                santa.isPlayerInRange = true;
            if (pumpkin != null)
                pumpkin.playerWithinRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (santa != null)
                santa.isPlayerInRange = false;
            if (pumpkin != null)
                pumpkin.playerWithinRange = false;
        }
    }
}
