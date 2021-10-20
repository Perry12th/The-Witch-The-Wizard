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
    public void StartGame()
    {
        SceneManager.LoadScene("IceLevel");
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
