using Assets.Scripts.Player.Data;
using UnityEngine;

public class ScoreCollector : MonoBehaviour
{
    private int score = 0;
    public int Score
    {
        get => score;

        private set
        {
            score = value;
            GameMenu.Instance.PlayerUI.UpdateScoreText(score);
        }
    }
    private int earnedPointsPerFlight = 0; // Очки, полученные за полет (За то время, пока скорость была достаточной для получения очков)

    private int currentMaxScoreMultiplierValue { get; set; } = 1; // Для сбора статистики

    private float counterScoreEarnedDelay;
    private readonly float scoreEarnedDelay = 0.25f;

    private readonly float scoreFontSize = 12f;

    private Rigidbody2D rb2D;
    public static float VelocityToCollectScore { get; private set; } = 25f; // По идее, скорость должна быть такая же, как при включении хвоста

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        GameMenu.Instance.PlayerUI.UpdateScoreText(score);
        GameMenu.Instance.GameOverScreen.GameOverStatusScreen.OnSavePlayerStats += SaveScoreStats;
    }


    private void OnDestroy()
    {
        GameMenu.Instance.GameOverScreen.GameOverStatusScreen.OnSavePlayerStats -= SaveScoreStats;
    }


    private void Update()
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

                Quaternion rotation = GameLogic.GetOrthoRotation(transform.position, GameManager.Instance.Centre.transform.position);
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


    private void SaveScoreStats()
    {
        PlayerStatsDataStorageSafe.Instance.SaveScoreData(Score);
        PlayerStatsDataStorageSafe.Instance.SaveScoreMultiplierData(currentMaxScoreMultiplierValue);
    }
}
