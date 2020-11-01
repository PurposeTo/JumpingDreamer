using UnityEngine;
using TMPro;

public class EarnedRewardsInGame : MonoBehaviour
{
    private TextMeshProUGUI earnedRewardsInGame;

    private int RecordUI => PlayerDataModelController.Instance.GetPlayerDataModel().PlayerStats.MaxEarnedScore.Value;

    private bool IsRecordNew => CurrentGameSessionData.Instance.IsRecordNew;


    // Awake вызывается при включении объекта
    private void Awake()
    {
        earnedRewardsInGame = gameObject.GetComponent<TextMeshProUGUI>();
        ShowScore();
    }


    private void ShowScore()
    {
        earnedRewardsInGame.text = GetScoreText();
    }


    public void ShowScoreWithRecord()
    {
        string recordScoreText;

        if (IsRecordNew) recordScoreText = $"New record!";
        else recordScoreText = $"Record";

        earnedRewardsInGame.text = $"{GetScoreText()}\n\n{recordScoreText}\n{RecordUI}";
    }


    private string GetScoreText()
    {
        int score = GameManager.Instance.PlayerPresenter.ScoreCollector.Score;
        string scoreText = $"Score\n{score}";

        int stars = GameManager.Instance.PlayerPresenter.StarCollector.Stars;
        string starsText = $"Stars\n{stars}";

        return $"{scoreText}\n{starsText}";
    }
}
