using UnityEngine;

public delegate void ScoreAmountChange();
public class ScoreCollector : MonoBehaviour
{
    public event ScoreAmountChange OnScoreAmountChange;

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

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        PlayerDataSaver.Instance.OnSavePlayerStats += SaveScoreStats;
    }


    private void OnDestroy()
    {
        PlayerDataSaver.Instance.OnSavePlayerStats -= SaveScoreStats;
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

                Quaternion rotation = GameLogic.GetOrthoRotation(transform.position, GameManager.Instance.CentreObject.transform.position);
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
        PlayerDataLocalStorageSafe.Instance.PlayerDataModel.PlayerStats.SaveScoreData(Score);
        PlayerDataLocalStorageSafe.Instance.PlayerDataModel.PlayerStats.SaveScoreMultiplierData(currentMaxScoreMultiplierValue);
    }
}
