using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SantaVisualBox : MonoBehaviour
{
    [SerializeField]
    SantaScript santa;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            santa.isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        santa.isPlayerInRange = false;
    }
}
