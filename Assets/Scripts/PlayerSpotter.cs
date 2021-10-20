using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpotter : MonoBehaviour
{
    public bool playerWithinRange { get; private set; }
    public WitcherScript player {get; private set; }
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        WitcherScript witcher = other.gameObject.GetComponent<WitcherScript>();
        if (witcher != null)
        {
            playerWithinRange = true;
            player = witcher;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player?.gameObject)
        {
            playerWithinRange = false;
            player = null;
        }
    }
}
