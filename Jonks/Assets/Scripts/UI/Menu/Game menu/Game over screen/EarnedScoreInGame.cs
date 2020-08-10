﻿using System;
using UnityEngine;
using TMPro;

public class EarnedScoreInGame : MonoBehaviour
{
    private TextMeshProUGUI earnedScore;

    private int RecordUI => PlayerStatsDataStorageSafe.Instance.PlayerStatsData.MaxEarnedScore;

    private bool isRecordNew = false;

    // Awake вызывается при включении объекта
    private void Awake()
    {
        earnedScore = gameObject.GetComponent<TextMeshProUGUI>();


        GameManager.Instance.PlayerPresenter.ScoreCollector.OnScoreAmountChange += ShowScore;
        GameManager.Instance.PlayerPresenter.StarCollector.OnStarAmountChange += ShowScore;
        ShowScore();


        /*
        * 1. Сохраняются статы
        * 2. При сохранении статов вызывается событие <Новый рекорд!> (Если рекорд новый)
        * 3. После сохранения статов вызывается метод ShowScoreWithRecord -> См. класс GameOverMenu
        */
        PlayerStatsDataStorageSafe.Instance.OnNewScoreRecord += delegate (object sender, EventArgs eventArgs) { isRecordNew = true; };
    }


    private void OnDestroy()
    {
        GameManager.Instance.PlayerPresenter.ScoreCollector.OnScoreAmountChange -= ShowScore;
        GameManager.Instance.PlayerPresenter.StarCollector.OnStarAmountChange -= ShowScore;
        // Отписаться от анонимного события!
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
}
