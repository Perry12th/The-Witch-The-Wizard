using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftScript : MonoBehaviour
{
    private List<CandyChairLiftScript> candyChairLifts = new List<CandyChairLiftScript>();
    private List<LiftTowerScript> liftTowers = new List<LiftTowerScript>();
    [SerializeField]
    private float chairLiftSpeed = 6.0f;
    [SerializeField]
    public bool isGoingLeft = true;
    [SerializeField]
    private LiftRoofScript liftRoof;
    [SerializeField]
    private LiftDropPointScript LeftliftDropPoint;
    [SerializeField]
    private LiftDropPointScript RightliftDropPoint;
    public bool isPowered = false;

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
        //if (!isPowered)
        //{
            WitcherScript player = FindObjectOfType<WitcherScript>();

            if (Vector3.Distance(player.transform.position, LeftliftDropPoint.transform.position) > Vector3.Distance(player.transform.position, RightliftDropPoint.transform.position))
            {
                isGoingLeft = true;
            }
            else
            {
                isGoingLeft = false;
            }
            foreach (CandyChairLiftScript candyChairLift in candyChairLifts)
            {
                candyChairLift.GetPathFollower().enabled = true;
                candyChairLift.GetPathFollower().SetSpeed(chairLiftSpeed);
                candyChairLift.FlipChair(isGoingLeft);
            }

            foreach (LiftTowerScript liftTower in liftTowers)
            {
                liftTower.OnPowered();
                liftTower.SwitchLighting(isGoingLeft);
            }
                isPowered = true;
        //}
    }

    public void PowerOff()
    {
        if (isPowered)
        {
            foreach (CandyChairLiftScript candyChairLift in candyChairLifts)
            {
                candyChairLift.GetPathFollower().enabled = false;
            }

            foreach (LiftTowerScript liftTower in liftTowers)
            {
                liftTower.OnPowerDown();
            }
            isPowered = false;
        }
    }

   
}
