using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyChairLiftScript : MonoBehaviour
{
    [SerializeField]
    private float zCorrection = -8;
    [SerializeField]
    private GameObject candyCaneParent;
    private WitcherScript player;
    [SerializeField]
    private PathCreation.Examples.PathFollower pathFollower;
    [SerializeField]
    private bool isGoingLeft = true;

    public void Update()
    {
        if (player != null)
        {
            if (player.GetIsLookingRight())
            {
                player.GetPlayerModel().transform.eulerAngles = new Vector3(0, 90.0f, 0);
            }
            else
            {
                player.GetPlayerModel().transform.eulerAngles = new Vector3(0, 270.0f, 0);
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
            player.GetRigidBody().useGravity = false;
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            if (player != null)
            {
                player.transform.parent = null;
                player.GetRigidBody().useGravity = true;
                //player.canMove = true;
                if (player.GetIsLookingRight())
                {
                    player.GetPlayerModel().transform.eulerAngles = new Vector3(0, 90.0f, 0);
                }
                else
                {
                    player.GetPlayerModel().transform.eulerAngles = new Vector3(0, 270.0f, 0);
                }
                player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, zCorrection);
                player = null;
            }
            
        }
    }

    public void FlipChair(bool goingLeft)
    {

        isGoingLeft = goingLeft;
        pathFollower.SetIsGoingLeft(goingLeft);
        float pathSpeed = pathFollower.GetSpeed();
        pathFollower.SetSpeed(goingLeft? Mathf.Abs(pathSpeed) : -Mathf.Abs(pathSpeed));
        if (isGoingLeft)
        {
            candyCaneParent.transform.localEulerAngles = new Vector3(candyCaneParent.transform.localEulerAngles.x, 270.0f, candyCaneParent.transform.localEulerAngles.z);
        }
        else if (!isGoingLeft)
        {
            candyCaneParent.transform.localEulerAngles = new Vector3(candyCaneParent.transform.localEulerAngles.x, 90.0f, candyCaneParent.transform.localEulerAngles.z);
        }
    }

    public PathCreation.Examples.PathFollower GetPathFollower()
    {
        return pathFollower;
    }
}
