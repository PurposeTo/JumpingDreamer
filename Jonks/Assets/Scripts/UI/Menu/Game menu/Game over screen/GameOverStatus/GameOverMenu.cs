using UnityEngine;
using TMPro;
using Assets.Scripts.Player.Data;

public delegate void SavePlayerStats();
public class GameOverMenu : MonoBehaviour
{
    private GameOverStatusScreen gameOverStatusScreen;
    public TextMeshProUGUI EarnedScore;

    public event SavePlayerStats OnSavePlayerStats;

    private string recordScoreText;
    private int record;


    private void Awake()
    {
        PlayerStatsDataStorageSafe.Instance.OnNewScoreRecord += SetNewBestScoreString;

        // Статистика должна сохраняться при появлении экрана GameOverMenu, но он появляется только один раз за все время существования игровой сцены. После этого сцена перезагружается => данный awake будет вызван уже после перезагрузки.
        OnSavePlayerStats?.Invoke();
    }


    private void OnEnable()
    {
        ShowBestScore();
    }


    private void OnDestroy()
    {
        PlayerStatsDataStorageSafe.Instance.OnNewScoreRecord -= SetNewBestScoreString;
    }


    private void SetNewBestScoreString()
    {
        record = PlayerStatsDataStorageSafe.Instance.PlayerStatsData.MaxEarnedScore;
        recordScoreText = $"New record!\n{record}";
    }


    private void ShowBestScore()
    {
        if (recordScoreText == null)
        {
            record = PlayerStatsDataStorageSafe.Instance.PlayerStatsData.MaxEarnedScore;
            recordScoreText = $"Record\n{record}";
        }

        EarnedScore.text += $"\n\n{recordScoreText}";
    }


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
