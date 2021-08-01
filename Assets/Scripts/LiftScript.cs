using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftScript : MonoBehaviour
{
    private List<CandyChairLiftScript> candyChairLifts = new List<CandyChairLiftScript>();
    private List<LiftTowerScript> liftTowers = new List<LiftTowerScript>();
    [SerializeField]
    private float chairLiftSpeed;
    private bool isPowered = false;

    // Start is called before the first frame update
    void Start()
    {
        candyChairLifts.AddRange(transform.GetComponentsInChildren<CandyChairLiftScript>());
        liftTowers.AddRange(transform.GetComponentsInChildren<LiftTowerScript>());

        foreach(CandyChairLiftScript candyChairLift in candyChairLifts)
        {
            candyChairLift.onLiftTowerPowered.AddListener(OnPoweredUp);
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
        }
    }
   
}
