using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuzyAnimatorScript : MonoBehaviour
{
    public WitcherScript ws;
    public void ReleaseFireball()
    {
        ws.ReleaseFireball();
    }

    public void Recover()
    {
        ws.Recover();
    }
}
