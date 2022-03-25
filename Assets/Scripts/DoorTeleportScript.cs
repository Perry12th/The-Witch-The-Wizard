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
            other.transform.position = new Vector3(targetDoor.exitPoint.position.x, targetDoor.exitPoint.position.y, other.transform.position.z); // This is for z-correction and prevent the player from becoming misaligned
        }
    }

}
