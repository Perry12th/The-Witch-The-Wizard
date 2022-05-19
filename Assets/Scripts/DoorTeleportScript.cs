using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTeleportScript : MonoBehaviour
{
    [SerializeField]
    Transform exitPoint; // The exit location that the player will exit to.

    [SerializeField]
    DoorTeleportScript targetDoor; // The door way the player will exit through.

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && targetDoor != null && exitPoint != null) // Check if the other collider is part of the player and there's a target door
        {
            other.transform.position = new Vector3(targetDoor.exitPoint.position.x, targetDoor.exitPoint.position.y, other.transform.position.z); // This is for z-correction and prevent the player from becoming misaligned
        }
    }

}
