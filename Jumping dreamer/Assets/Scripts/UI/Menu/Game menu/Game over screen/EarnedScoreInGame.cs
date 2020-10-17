using System;
using UnityEngine;
using TMPro;

public class EarnedScoreInGame : MonoBehaviour
{
    private TextMeshProUGUI earnedScore;

    private int RecordUI => PlayerDataModelController.Instance.GetPlayerDataModel().PlayerStats.MaxEarnedScore.Value;

    private bool isRecordNew = false;
    private Action isRecordNewEvent = null;


    // Awake вызывается при включении объекта
    private void Awake()
    {
        earnedScore = gameObject.GetComponent<TextMeshProUGUI>();

        GameManager.Instance.PlayerPresenter.ScoreCollector.OnScoreAmountChange += UpdateResults;
        GameManager.Instance.PlayerPresenter.StarCollector.OnStarAmountChange += UpdateResults;
        ShowScore();

        /*
        * 1. Сохраняются статы
        * 2. При сохранении статов вызывается событие <Новый рекорд!> (Если рекорд новый)
        * 3. После сохранения статов вызывается метод ShowScoreWithRecord -> См. класс GameOverMenu
        */
        isRecordNewEvent = () => isRecordNew = true;
        PlayerDataModelController.Instance.GetPlayerDataModel().PlayerStats.OnNewScoreRecord += isRecordNewEvent;
    }


    private void OnDestroy()
    {
        GameManager.Instance.PlayerPresenter.ScoreCollector.OnScoreAmountChange -= UpdateResults;
        GameManager.Instance.PlayerPresenter.StarCollector.OnStarAmountChange -= UpdateResults;
        PlayerDataModelController.Instance.GetPlayerDataModel().PlayerStats.OnNewScoreRecord -= isRecordNewEvent;
    }


    private void ShowScore()
    {
        int score = GameManager.Instance.PlayerPresenter.ScoreCollector.Score;
        string scoreText = $"Score\n{score}";

        int stars = GameManager.Instance.PlayerPresenter.StarCollector.Stars;
        string starsText = $"Stars\n{stars}";

        earnedScore.text = $"{scoreText}\n{starsText}";
    }


    public void ShowScoreWithRecord()
    {
        string recordScoreText;

        if (isRecordNew)
        {
            recordScoreText = $"New record!";
        }
        else
        {
            recordScoreText = $"Record";
        }

        earnedScore.text += $"\n\n{recordScoreText}\n{RecordUI}";
    }


    private void UpdateResults()
    {
        ShowScore();
        ShowScoreWithRecord();
    }
}
