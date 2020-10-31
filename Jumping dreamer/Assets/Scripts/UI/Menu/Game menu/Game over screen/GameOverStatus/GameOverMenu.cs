using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    private EarnedRewardsInGame EarnedRewardsInGame => GameMenu.Instance.GameOverScreen.EarnedRewardsInGame;

    private GPGSLeaderboard GPGSLeaderboard => GPGSLeaderboard.Instance;

    private void Awake()
    {
        // Статистика должна сохраняться при появлении экрана GameOverMenu, но он появляется только один раз за все время существования игровой сцены. После этого сцена перезагружается => данный Awake будет вызван уже после перезагрузки.
        PlayerDataModelController.Instance.UpdatePlayerModelAndSavePlayerData();
        UnityEngine.Debug.Log($"ScoreDebug: GameOverMenu awake call.");
    }


    // Руками не трогать - вызов метода должен происходить после инициализации скрипта EarnedScoreInGame
    private void Start()
    {
        EarnedRewardsInGame.ShowScoreWithRecord();
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


    public void OpenLeaderboardClickHandler()
    {
        GPGSLeaderboard.OpenLeaderboard();
    }
}
