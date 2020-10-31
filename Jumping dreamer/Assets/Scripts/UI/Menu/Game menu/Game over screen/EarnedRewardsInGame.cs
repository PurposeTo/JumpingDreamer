using System;
using UnityEngine;
using TMPro;

public class EarnedRewardsInGame : MonoBehaviour
{
    private TextMeshProUGUI earnedRewardsInGame;

    private int RecordUI => PlayerDataModelController.Instance.GetPlayerDataModel().PlayerStats.MaxEarnedScore.Value;

    private bool IsRecordNew => CurrentGameSessionData.Instance.IsRecordNew;


    private Action OnChangedEarnedRewardsInGame; // При изменении счета или звезд должны обновить текст используя актуальный метод отображения

    // Awake вызывается при включении объекта
    private void Awake()
    {
        earnedRewardsInGame = gameObject.GetComponent<TextMeshProUGUI>();

        CurrentGameSessionData.Instance.OnScoreOrStarsChanged += OnChangedEarnedRewardsInGameCall;
        CurrentGameSessionData.Instance.OnNewRecordScore += OnChangedEarnedRewardsInGameCall;

        ShowScore();
    }


    private void OnDestroy()
    {
        CurrentGameSessionData.Instance.OnScoreOrStarsChanged -= OnChangedEarnedRewardsInGameCall;
        CurrentGameSessionData.Instance.OnNewRecordScore -= OnChangedEarnedRewardsInGameCall;
    }


    private void OnChangedEarnedRewardsInGameCall()
    {
        OnChangedEarnedRewardsInGame?.Invoke();
    }


    private void ShowScore()
    {
        OnChangedEarnedRewardsInGame = ShowScore;
        earnedRewardsInGame.text = GetScoreText();
    }


    public void ShowScoreWithRecord()
    {
        OnChangedEarnedRewardsInGame = ShowScoreWithRecord;

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
