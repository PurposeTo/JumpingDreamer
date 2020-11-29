using UnityEngine;
using TMPro;

public class EarnedRewardsInGame : MonoBehaviour
{
    private TextMeshProUGUI earnedRewardsInGame;

    private int RecordUI => PlayerDataModelController.Instance.GetGettableDataModel().PlayerStats.MaxEarnedScore.Value;

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
        int score = GameObjectsHolder.Instance.PlayerPresenter.ScoreCollector.Score;
        string scoreText = $"Score\n{score}";

        int stars = GameObjectsHolder.Instance.PlayerPresenter.StarCollector.Stars;
        string starsText = $"Stars\n{stars}";

        return $"{scoreText}\n{starsText}";
    }
}
