﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool gameIsPaused { get; private set; }
    public CheckpointScript currentCheckpoint { get; private set; }

    #region Singleton
    public static GameManager instance;

    private void Awake()
    {
        //Make sure there is only one instance
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    public void SwitchScenes(string sceneName)
    {
        if (gameIsPaused)
        {
            UnPauseGame();
        }
        SceneManager.LoadScene(sceneName);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        gameIsPaused = true;
    }

    public void UnPauseGame()
    {
        Time.timeScale = 1;
        gameIsPaused = false;
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;    
        #else
            Application.Quit();
        #endif
    }

    public void SetCheckpoint(CheckpointScript newCheckpoint)
    {
        if (currentCheckpoint != newCheckpoint)
        {
            currentCheckpoint?.disableCheckpoint();
        }
        newCheckpoint.enableCheckpoint();

        currentCheckpoint = newCheckpoint;
    }
}
