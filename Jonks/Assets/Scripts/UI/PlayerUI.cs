using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI CoinsText;
    private string coinsDefaultText = "Coins: ";

    public TextMeshProUGUI ScoreText;
    private string scoreDefaultText = "Score: ";


    public void UpdateCoinsText(int value)
    {
        CoinsText.text = coinsDefaultText + value;
    }


    public void UpdateScoreText(int value)
    {
        ScoreText.text = scoreDefaultText + value;
    }
}
