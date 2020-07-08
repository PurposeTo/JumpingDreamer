using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    private GameOverStatusScreen gameOverStatusScreen;

    public void Initialize(GameOverStatusScreen gameOverStatusScreen)
    {
        this.gameOverStatusScreen = gameOverStatusScreen;
    }


    public void ReloadLvl()
    {
        SceneLoader.LoadScene(SceneLoader.GameSceneName);
    }


    public void OpenMainMenu()
    {
        SceneLoader.LoadScene(SceneLoader.MainMenuName);
    }


    public void OpenShop() { }


    public void OpenLeaderboard() { }
}
