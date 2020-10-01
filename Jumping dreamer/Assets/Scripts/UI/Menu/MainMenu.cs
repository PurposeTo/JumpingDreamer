using UnityEngine;
using TMPro;

public class MainMenu : SingletonMonoBehaviour<MainMenu>
{
    public SettingsMenu SettingsMenu;

    private GPGSLeaderboard GPGSLeaderboard => GPGSServices.Instance.GPGSLeaderboard;

    public void StartGameClickHandler()
    {
        SceneLoader.LoadScene(SceneLoader.GameSceneName);
    }


    public void OpenLeaderboardClickHandler()
    {
        GPGSLeaderboard.OpenLeaderboard();
    }
}