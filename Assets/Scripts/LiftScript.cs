using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftScript : MonoBehaviour
{
    private List<CandyChairLiftScript> candyChairLifts = new List<CandyChairLiftScript>();
    private List<LiftTowerScript> liftTowers = new List<LiftTowerScript>();
    [SerializeField]
    private float chairLiftSpeed;
    [SerializeField]
    private bool isGoingLeft = true;
    [SerializeField]
    private LiftRoofScript liftRoof;
    private bool isPowered = false;
    private bool canTurn = false;
    [SerializeField]
    private float turnRefreshTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        candyChairLifts.AddRange(transform.GetComponentsInChildren<CandyChairLiftScript>());
        liftTowers.AddRange(transform.GetComponentsInChildren<LiftTowerScript>());

        foreach(CandyChairLiftScript candyChairLift in candyChairLifts)
        {
            liftRoof.onLiftTowerPowered.AddListener(OnPoweredUp);
        }
    }

    private void OnPoweredUp()
    {

        if (!isPowered)
        {
            foreach (CandyChairLiftScript candyChairLift in candyChairLifts)
            {
                candyChairLift.pathFollower.enabled = true;
                candyChairLift.pathFollower.speed = chairLiftSpeed;
            }

            foreach (LiftTowerScript liftTower in liftTowers)
            {
                liftTower.OnPowered();
            }
                isPowered = true;
            StartCoroutine(RefreshTurn());
            return;
        }
        else if (isPowered && canTurn)
        {
            isGoingLeft = !isGoingLeft;
            canTurn = false;
            StartCoroutine(RefreshTurn());

            foreach (CandyChairLiftScript candyChairLift in candyChairLifts)
            {
                candyChairLift.FlipChair();
            }

            foreach (LiftTowerScript liftTower in liftTowers)
            {
                liftTower.SwitchLighting();
            }
        }
    }

    private IEnumerator RefreshTurn()
    {
        yield return new WaitForSeconds(turnRefreshTime);
        canTurn = true;
    }
   
}
