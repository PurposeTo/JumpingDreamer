using UnityEngine;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    public TextMeshProUGUI EarnedScore;
    public TextMeshProUGUI EarnedCoins;
    public TextMeshProUGUI LastRecord;


    private void OnEnable()
    {
        GameMenu.Instance.PlayerUI.gameObject.SetActive(false);
        GameMenu.Instance.PauseMenu.PauseButton.SetActive(false);
        ShowScore();
    }


    private void OnDisable()
    {
        GameMenu.Instance.PlayerUI.gameObject.SetActive(true);
        GameMenu.Instance.PauseMenu.PauseButton.SetActive(true);
    }


    private void ShowScore()
    {
        int score = GameManager.Instance.Player.GetComponent<ScoreCollector>().Score;
        EarnedScore.text = $"{score}";

        int coins = GameManager.Instance.Player.GetComponent<CoinCollector>().Coins;
        EarnedCoins.text = $"{coins}";
    }
}
