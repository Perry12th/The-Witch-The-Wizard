using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CandyChairLiftScript : MonoBehaviour, IElectrical
{
    public UnityEvent onLiftTowerPowered;
    private bool isPowered = false;
    private WitcherScript player;
    [SerializeField] List<GameObject> dropOffPoints = new List<GameObject>();
    [SerializeField] MeshCollider chairCollider;
    [SerializeField] float zCorrection;


    public PathCreation.Examples.PathFollower pathFollower;
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && isPowered)
        {
            Debug.Log("Entered Seat");
            player = collision.gameObject.GetComponent<WitcherScript>();
            player.transform.parent = transform;
            player.canMove = false;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (player != null)
            {
                player.transform.parent = null;
                player.canMove = true;
            }
            player = null;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit TriggerEnter");
        foreach(GameObject dropPoint in dropOffPoints)
        {
            if(other.gameObject == dropPoint && player != null)
            {
                // Chair lift has enter one of the drop off points with the player onboard
                player.transform.parent = null;
                chairCollider.enabled = false;
                player.canMove = true;
                if (player.lookingRight)
                {
                    player.model.transform.eulerAngles = new Vector3(0, 90.0f, 0);
                }
                else
                {
                    player.model.transform.eulerAngles = new Vector3(0, 270.0f, 0);
                }
                player = null;
                return;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log("Hit TriggerExit");
        foreach (GameObject dropPoint in dropOffPoints)
        {
            if (other.gameObject == dropPoint && !chairCollider.enabled)
            {
                // ChairLift has left the drop off point after dropping off the player
                chairCollider.enabled = true;
                return;
            }
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
