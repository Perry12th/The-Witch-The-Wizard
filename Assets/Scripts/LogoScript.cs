using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoScript : MonoBehaviour
{
    [SerializeField]
    private string nextSceneName;
   public void LoadMainMenu()
    {
        GameManager.instance.SwitchScenes(nextSceneName);
    }
}
