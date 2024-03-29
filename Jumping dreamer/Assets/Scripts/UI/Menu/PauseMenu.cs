﻿using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject PauseButton;
    public GameObject PauseScreen;

    public void Pause()
    {
        GameManager.Instance.SetPlayerPause(true);

        PauseScreen.SetActive(true);
        PauseButton.SetActive(false);
    }


    public void Resume()
    {
        GameManager.Instance.SetPlayerPause(false);

        PauseScreen.SetActive(false);
        PauseButton.SetActive(true);
    }


    public void OpenMainMenu()
    {
        SingleSceneLoader.Instance.LoadScene(SingleSceneLoader.MainMenuName);
    }
}
