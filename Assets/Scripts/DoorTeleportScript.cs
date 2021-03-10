using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTeleportScript : MonoBehaviour
{
    [SerializeField]
    Transform exitPoint;

    [SerializeField]
    DoorTeleportScript targetDoor;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = targetDoor.exitPoint.position;
        }
    }

}
