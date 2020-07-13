using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI StarsText;
    private string starsDefaultText = "Stars: ";

    public TextMeshProUGUI ScoreText;
    private string scoreDefaultText = "Score: ";


    public void UpdateStarsText(int value)
    {
        StarsText.text = starsDefaultText + value;
    }


    public void UpdateScoreText(int value)
    {
        ScoreText.text = scoreDefaultText + value;
    }
}
