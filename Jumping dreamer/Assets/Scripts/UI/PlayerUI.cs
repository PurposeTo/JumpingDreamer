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
        GameObjectsHolder.Instance.PlayerPresenter.ScoreCollector.OnScoreAmountChange += UpdateScoreText;
        GameObjectsHolder.Instance.PlayerPresenter.StarCollector.OnStarAmountChange += UpdateStarsText;

        UpdateStarsText();
        UpdateScoreText();
    }


    private void OnDestroy()
    {
        GameObjectsHolder.Instance.PlayerPresenter.ScoreCollector.OnScoreAmountChange -= UpdateScoreText;
        GameObjectsHolder.Instance.PlayerPresenter.StarCollector.OnStarAmountChange -= UpdateStarsText;
    }


    public void UpdateStarsText()
    {
        int value = GameObjectsHolder.Instance.PlayerPresenter.StarCollector.Stars;
        StarsText.text = starsDefaultText + value;
    }


    public void UpdateScoreText()
    {
        int value = GameObjectsHolder.Instance.PlayerPresenter.ScoreCollector.Score;
        ScoreText.text = scoreDefaultText + value;
    }
}
