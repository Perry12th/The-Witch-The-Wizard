using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject startScreen;
    [SerializeField]
    private GameObject creditsScreen;
    [SerializeField]
    private string firstLevel;
    [SerializeField]
    private string iceLevel;

    public void LoadFirstLevel()
    {
        GameManager.instance.SwitchScenes(firstLevel);
    }

    public void LoadIceLevel()
    {
        GameManager.instance.SwitchScenes(iceLevel);
    }

    public void OpenStartScreen()
    {
        creditsScreen.SetActive(false);
        startScreen.SetActive(true);
    }

    public void OpenCreditsScreen()
    {
        creditsScreen.SetActive(true);
        startScreen.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
