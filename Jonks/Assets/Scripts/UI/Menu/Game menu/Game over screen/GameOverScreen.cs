using UnityEngine;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    public TextMeshProUGUI EarnedScore;
    public GameOverStatusScreen GameOverStatusScreen;


    private void Start()
    {
        GameManager.Instance.PlayerPresenter.ScoreCollector.OnScoreAmountChange += ShowScore;
        GameManager.Instance.PlayerPresenter.StarCollector.OnStarAmountChange += ShowScore;
    }


    private void OnDestroy()
    {
        GameManager.Instance.PlayerPresenter.ScoreCollector.OnScoreAmountChange -= ShowScore;
        GameManager.Instance.PlayerPresenter.StarCollector.OnStarAmountChange -= ShowScore;
    }


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
        int score = GameManager.Instance.PlayerPresenter.ScoreCollector.Score;
        string scoreText = $"Score\n{score}";

        int stars = GameManager.Instance.PlayerPresenter.StarCollector.Stars;
        string starsText = $"Stars\n{stars}";

        EarnedScore.text = $"{scoreText}\n{starsText}";
    }
}
