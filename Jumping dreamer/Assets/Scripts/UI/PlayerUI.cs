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
        ImportantGameObjectsHolder.Instance.PlayerPresenter.ScoreCollector.OnScoreAmountChange += UpdateScoreText;
        ImportantGameObjectsHolder.Instance.PlayerPresenter.StarCollector.OnStarAmountChange += UpdateStarsText;

        UpdateStarsText();
        UpdateScoreText();
    }


    private void OnDestroy()
    {
        ImportantGameObjectsHolder.Instance.PlayerPresenter.ScoreCollector.OnScoreAmountChange -= UpdateScoreText;
        ImportantGameObjectsHolder.Instance.PlayerPresenter.StarCollector.OnStarAmountChange -= UpdateStarsText;
    }


    public void UpdateStarsText()
    {
        int value = ImportantGameObjectsHolder.Instance.PlayerPresenter.StarCollector.Stars;
        StarsText.text = starsDefaultText + value;
    }


    public void UpdateScoreText()
    {
        int value = ImportantGameObjectsHolder.Instance.PlayerPresenter.ScoreCollector.Score;
        ScoreText.text = scoreDefaultText + value;
    }
}
