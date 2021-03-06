using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuzyAnimatorScript : MonoBehaviour
{
    [SerializeField]
    private WitcherScript ws;
    public void ReleaseFireball()
    {
        ws.ReleaseFireball();
    }

    public void Recover()
    {
        ws.Recover();
    }

    public void FireLighting()
    {
        ws.FireLighting();
    }

    public void LightingRecover()
    {
        ws.RecoverLighting();
    }

    public void FireSnowBall()
    {
        ws.FireSnowBall();
    }

    public void RecoverSnowBall()
    {
        ws.RecoverSnowBall();
    }
}
