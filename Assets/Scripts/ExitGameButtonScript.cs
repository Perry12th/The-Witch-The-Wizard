using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitGameButtonScript : MonoBehaviour
{
    public void ExitGame()
    {
        AppHelper.Quit();
    }
}
