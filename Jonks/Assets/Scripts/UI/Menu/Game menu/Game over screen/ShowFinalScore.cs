using UnityEngine;
using TMPro;

public class ShowFinalScore : MonoBehaviour
{
    public TextMeshProUGUI EarnedScore;


    private void Awake()
    {

    }


    private void OnDestroy()
    {

    }


    private void ShowScore()
    {
        int score = GameManager.Instance.PlayerPresenter.ScoreCollector.Score;
        string scoreText = $"Score\n{score}";

        int stars = GameManager.Instance.PlayerPresenter.StarCollector.Stars;
        string starsText = $"Stars\n{stars}";

        EarnedScore.text = $"{scoreText}\n{starsText}";
    }


    private void ShowScoreWithRecord(bool isRecordNew)
    {

    }
}
