using UnityEngine;
using TMPro;
using Assets.Scripts.Player.Data;

public class GameOverScreen : MonoBehaviour
{
    public TextMeshProUGUI EarnedScore;
    public GameOverStatusScreen GameOverStatusScreen;


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
        string scoreText = $"Score\n{score}";

        int stars = GameManager.Instance.Player.GetComponent<StarCollector>().Stars;
        string starsText = $"Stars\n{stars}";

        int record = PlayerStatsDataStorageSafe.Instance.PlayerStatsDataModel.maxEarnedPointsAmount;
        string recordScoreText = $"Last record\n{record}"; //Показывать прошлый или текущий рекорд?

        EarnedScore.text = $"{scoreText}\n{starsText}\n\n{recordScoreText}";
    }
}
