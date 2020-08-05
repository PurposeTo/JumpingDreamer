﻿using UnityEngine;
using TMPro;

public class MainMenu : SingletonMonoBehaviour<MainMenu>
{
    public SettingsMenu SettingsMenu;

    public void StartGameClickHandler()
    {
        SceneLoader.LoadScene(SceneLoader.GameSceneName);
    }
}