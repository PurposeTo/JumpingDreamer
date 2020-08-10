using UnityEngine;
using TMPro;
using System;

public delegate void SavePlayerStats();
public class GameOverMenu : MonoBehaviour
{
    private GameOverStatusScreen gameOverStatusScreen;
    public TextMeshProUGUI EarnedScore;

    public event SavePlayerStats OnSavePlayerStats;

    private string recordScoreText;
    private int RecordUI => PlayerStatsDataStorageSafe.Instance.PlayerStatsData.MaxEarnedScore;


    private void Awake()
    {
        PlayerStatsDataStorageSafe.Instance.OnNewScoreRecord += SetNewBestScoreString;

        // Статистика должна сохраняться при появлении экрана GameOverMenu, но он появляется только один раз за все время существования игровой сцены. После этого сцена перезагружается => данный awake будет вызван уже после перезагрузки.
        OnSavePlayerStats?.Invoke();
    }


    private void OnDestroy()
    {
        PlayerStatsDataStorageSafe.Instance.OnNewScoreRecord -= SetNewBestScoreString;
    }


    private void OnEnable()
    {
        ShowBestScore();
    }


    public void Initialize(GameOverStatusScreen gameOverStatusScreen)
    {
        this.gameOverStatusScreen = gameOverStatusScreen;
    }


    private void SetNewBestScoreString(object sender, EventArgs eventArgs)
    {
        recordScoreText = $"New record!\n{RecordUI}";
    }


    private void ShowBestScore()
    {
        if (recordScoreText == null)
        {
            recordScoreText = $"Record\n{RecordUI}";
        }

        EarnedScore.text += $"\n\n{recordScoreText}";
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
