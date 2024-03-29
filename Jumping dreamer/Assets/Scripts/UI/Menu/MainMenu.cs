﻿using UnityEngine;
using TMPro;
using Desdiene.Singleton;

public class MainMenu : SingletonSuperMonoBehaviour<MainMenu>
{
    public SettingsMenu SettingsMenu;

    private GPGSLeaderboard GPGSLeaderboard => GPGSLeaderboard.Instance;

    public void StartGameClickHandler()
    {
        SingleSceneLoader.Instance.LoadScene(SingleSceneLoader.GameSceneName);
    }


    public void OpenLeaderboardClickHandler()
    {
        GPGSLeaderboard.OpenLeaderboard();
    }
}