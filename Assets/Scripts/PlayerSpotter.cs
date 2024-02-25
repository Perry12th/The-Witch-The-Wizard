using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSpotter : MonoBehaviour
{
    public bool playerWithinRange { get; private set; }
    public WitcherScript player {get; private set; }
    // Start is called before the first frame update

    public UnityAction playerSpotted;
    public UnityAction playerLeft;
    private void OnTriggerEnter(Collider other)
    {
        WitcherScript witcher = other.gameObject.GetComponent<WitcherScript>();
        if (witcher != null)
        {
            playerWithinRange = true;
            player = witcher;
            playerSpotted?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (player == null || other.gameObject != player.gameObject) return;
        playerWithinRange = false;
        player = null;
        playerLeft?.Invoke();
    }
}
