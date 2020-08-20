using System;
using UnityEngine;

public delegate void SavePlayerStats();
public class GameOverMenu : MonoBehaviour
{
    public EarnedScoreInGame EarnedScoreInGame;

    public event SavePlayerStats OnSavePlayerStats;
    public event Action<bool> OnToggleGameRunningState;


    private void Awake()
    {
        // Статистика должна сохраняться при появлении экрана GameOverMenu, но он появляется только один раз за все время существования игровой сцены. После этого сцена перезагружается => данный Awake будет вызван уже после перезагрузки.
        OnSavePlayerStats?.Invoke();
        OnToggleGameRunningState?.Invoke(false);
        EarnedScoreInGame.ShowScoreWithRecord();
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
