using UnityEngine;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    public TextMeshProUGUI EarnedScore;
    public TextMeshProUGUI EarnedCoins;
    public TextMeshProUGUI LastRecord;


    private void OnEnable()
    {
        GameMenu.Instance.PauseMenu.PauseButton.SetActive(false);
        ShowScore();
    }


    private void ShowScore()
    {
        int score = GameManager.Instance.Player.GetComponent<ScoreCollector>().Score;
        EarnedScore.text = $"{score}";

        int coins = GameManager.Instance.Player.GetComponent<CoinCollector>().Coins;
        EarnedCoins.text = $"{coins}";
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
