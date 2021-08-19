using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyChairLiftScript : MonoBehaviour
{
    [SerializeField]
    private float zCorrection = -8;
    private WitcherScript player;
    public PathCreation.Examples.PathFollower pathFollower;
    public bool isGoingLeft = true;

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

    public void FlipChair(bool goingLeft)
    {

        isGoingLeft = goingLeft;
        pathFollower.isGoingLeft = goingLeft;
        pathFollower.speed = goingLeft? Mathf.Abs(pathFollower.speed) : -Mathf.Abs(pathFollower.speed);
        if (isGoingLeft)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0.0f);
        }
        else if (!isGoingLeft)
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 180.0f);
        }
    }
}
