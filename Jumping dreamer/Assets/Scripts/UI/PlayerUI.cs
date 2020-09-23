using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI StarsText;
    private string starsDefaultText = "Stars: ";

    public TextMeshProUGUI ScoreText;
    private string scoreDefaultText = "Score: ";


    private void Start()
    {
        GameManager.Instance.PlayerPresenter.ScoreCollector.OnScoreAmountChange += UpdateScoreText;
        GameManager.Instance.PlayerPresenter.StarCollector.OnStarAmountChange += UpdateStarsText;

        UpdateStarsText();
        UpdateScoreText();
    }


    private void OnDestroy()
    {
        GameManager.Instance.PlayerPresenter.ScoreCollector.OnScoreAmountChange -= UpdateScoreText;
        GameManager.Instance.PlayerPresenter.StarCollector.OnStarAmountChange -= UpdateStarsText;
    }


    public void UpdateStarsText()
    {
        int value = GameManager.Instance.PlayerPresenter.StarCollector.Stars;
        StarsText.text = starsDefaultText + value;
    }


    public void UpdateScoreText()
    {
        int value = GameManager.Instance.PlayerPresenter.ScoreCollector.Score;
        ScoreText.text = scoreDefaultText + value;
    }
}
