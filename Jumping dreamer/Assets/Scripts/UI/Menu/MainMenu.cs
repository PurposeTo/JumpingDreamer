using UnityEngine;
using TMPro;

public class MainMenu : SingletonMonoBehaviour<MainMenu>
{
    public SettingsMenu SettingsMenu;

    private GPGSLeaderboard GPGSLeaderboard => GPGSLeaderboard.Instance;

    public void StartGameClickHandler()
    {
        SingleSceneLoader.LoadScene(SingleSceneLoader.GameSceneName);
    }


    public void OpenLeaderboardClickHandler()
    {
        GPGSLeaderboard.OpenLeaderboard();
    }
}