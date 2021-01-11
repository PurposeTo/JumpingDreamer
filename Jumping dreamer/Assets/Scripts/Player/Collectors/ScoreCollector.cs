using System;
using UnityEngine;

public class ScoreCollector : RewardCollector
{
    public event Action OnScoreAmountChange;

    private SafeInt score = 0;
    public SafeInt Score
    {
        get => score;

        private set
        {
            score = value;
            OnScoreAmountChange?.Invoke();
        }
    }
    private SafeInt earnedPointsPerFlight = 0; // Очки, полученные за полет (За то время, пока скорость была достаточной для получения очков)

    private SafeInt currentMaxScoreMultiplierValue = 1; // Для сбора статистики

    private float counterScoreEarnedDelay;
    private readonly float scoreEarnedDelay = 0.25f;

    private readonly float scoreFontSize = 12f;

    private Rigidbody2D rb2D;
    public static float VelocityToCollectScore { get; private set; } = 25f; // По идее, скорость должна быть такая же, как при включении хвоста


    private void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }


    private protected override void Start()
    {
        base.Start();
        PlayerDataModelController.Instance.SynchronizerNotifier.OnSavePlayerData += SaveScoreStats;
    }


    private protected override void OnDestroy()
    {
        base.OnDestroy();
        PlayerDataModelController.Instance.SynchronizerNotifier.OnSavePlayerData -= SaveScoreStats;
    }


    private void Update()
    {
        if (canCollecting) CollectScore();
    }


    private void SaveScoreStats()
    {
        PlayerDataModelController.Instance.DataInteraction.Setter.Stats.SaveRecordScore(Score);
        PlayerDataModelController.Instance.DataInteraction.Setter.Stats.SaveRecordScoreMultiplier(currentMaxScoreMultiplierValue);
    }


    private void CollectScore()
    {
        if (rb2D.velocity.magnitude >= VelocityToCollectScore)
        {
            if (counterScoreEarnedDelay > 0f)
            {
                counterScoreEarnedDelay -= Time.deltaTime;
            }
            else
            {
                // Арифметическая прогрессия получения очков
                earnedPointsPerFlight++;
                Score += earnedPointsPerFlight;

                if (earnedPointsPerFlight > currentMaxScoreMultiplierValue)
                {
                    currentMaxScoreMultiplierValue = earnedPointsPerFlight;
                }

                Quaternion rotation = GameLogic.GetOrthoRotation(transform.position, GameObjectsHolder.Instance.Centre.gameObject.transform.position);
                VFXManager.Instance.DisplayPopupText(transform.position, rotation, $"+{earnedPointsPerFlight}", Color.white, scoreFontSize);

                counterScoreEarnedDelay = scoreEarnedDelay;
            }
        }
        else
        {
            earnedPointsPerFlight = 0;
            counterScoreEarnedDelay = -1f; // Счет всегда должен включаться сразу же, как только скорость будет нужной
        }
    }
}
