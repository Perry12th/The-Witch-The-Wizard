using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyChairLiftScript : MonoBehaviour
{
    WitcherScript player;
    GameObject inDropOffPoint = null;
    [SerializeField]
    bool isGoingLeft = true;
    [SerializeField]
    GameObject leftDropPoint;
    [SerializeField]
    GameObject rightDropPoint;
    [SerializeField] 
    Collider chairColliderA;
    [SerializeField] 
    Collider chairColliderB;
    [SerializeField] 
    float zCorrection;
    [SerializeField]
    Material chairMaterial;
    [SerializeField]
    Material fadeMaterial;
    [SerializeField]
    MeshRenderer meshRenderer;
    public PathCreation.Examples.PathFollower pathFollower;

    public void Update()
    {
        if (player != null)
        {
            if (player.lookingRight)
            {
                player.model.transform.eulerAngles = new Vector3(0, 90.0f, 0);
            }
            else
            {
                player.model.transform.eulerAngles = new Vector3(0, 270.0f, 0);
            }
        }
    }
    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.transform.gameObject.name);
        WitcherScript witcher = collision.gameObject.GetComponent<WitcherScript>();
        if (witcher != null)
        {
            Debug.Log("Entered Seat");

            player = witcher;
            player.transform.parent = transform;
        }
    }

    //public void OnCollisionStay(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player") && isPowered)
    //    {
    //        player = collision.gameObject.GetComponent<WitcherScript>();
    //        player.transform.parent = transform;
    //    }
    //}

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (player != null)
            {
                player.transform.parent = null;
                //player.canMove = true;
                if (player.lookingRight)
                {
                    player.model.transform.eulerAngles = new Vector3(0, 90.0f, 0);
                }
                else
                {
                    player.model.transform.eulerAngles = new Vector3(0, 270.0f, 0);
                }
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, zCorrection);
                player = null;
            }
            
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit TriggerEnter");
        //foreach(GameObject dropPoint in dropOffPoints)
        //{
        //    if(other.gameObject == dropPoint && player != null)
        //    {
        //        // Chair lift has enter one of the drop off points with the player onboard
        //        player.transform.parent = null;
        //        chairCollider.enabled = false;
        //        player.canMove = true;
        //        if (player.lookingRight)
        //        {
        //            player.model.transform.eulerAngles = new Vector3(0, 90.0f, 0);
        //        }
        //        else
        //        {
        //            player.model.transform.eulerAngles = new Vector3(0, 270.0f, 0);
        //        }
        //        player = null;
        //        return;
        //    }
        //}
        //if (other.gameObject == leftDropPoint || other.gameObject == rightDropPoint)
        //{
        //    inDropOffPoint = other.gameObject;
        //    if (player != null && isGoingLeft && inDropOffPoint == leftDropPoint || !isGoingLeft && inDropOffPoint == rightDropPoint)
        //    {
        //        chairColliderA.enabled = false;
        //        chairColliderB.enabled = false;
        //        meshRenderer.material = fadeMaterial;
        //    }
        //    else
        //    {
        //        chairColliderA.enabled = true;
        //        chairColliderB.enabled = true;
        //        meshRenderer.material = chairMaterial;
        //    }

        //}
        //if (player != null)
        //{
        //    //Chair lift has enter one of the drop off points with the player onboard
        //    player.transform.parent = null;
        //    //player.canMove = true;
        //    if (player.lookingRight)
        //    {
        //        player.model.transform.eulerAngles = new Vector3(0, 90.0f, 0);
        //    }
        //    else
        //    {
        //        player.model.transform.eulerAngles = new Vector3(0, 270.0f, 0);
        //    }
        //    player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, zCorrection);
        //    player = null;
        //}
    }

    public void OnTriggerExit(Collider other)
    {
        Debug.Log("Hit TriggerExit");
        //foreach (GameObject dropPoint in dropOffPoints)
        //{
        //    if (other.gameObject == dropPoint && !chairCollider.enabled)
        //    {
        //        // ChairLift has left the drop off point after dropping off the player
        //        chairCollider.enabled = true;
        //        return;
        //    }
        //}
        //if (other.gameObject == leftDropPoint || other.gameObject == rightDropPoint)
        //{
        //   inDropOffPoint = null;
        //   chairColliderA.enabled = true;
        //   chairColliderB.enabled = true;
        //   meshRenderer.material = chairMaterial;
        //}
    }


    public void FlipChair()
    {
        isGoingLeft = !isGoingLeft;
        pathFollower.isGoingLeft = isGoingLeft;
        pathFollower.speed = -pathFollower.speed;
        //chairColliderA.enabled = true;
        //chairColliderB.enabled = true;
        //meshRenderer.material = chairMaterial;

    }
}
