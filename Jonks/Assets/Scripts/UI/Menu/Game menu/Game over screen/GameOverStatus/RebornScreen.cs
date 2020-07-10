using UnityEngine;

public class RebornScreen : MonoBehaviour
{
    private GameOverStatusScreen gameOverStatusScreen;

    public void Initialize(GameOverStatusScreen gameOverStatusScreen)
    {
        this.gameOverStatusScreen = gameOverStatusScreen;
    }


    public void Reborn()
    {
        gameOverStatusScreen.isPlayerMustSeeAd = true;
        // Возродить

        GameMenu.Instance.GameOverScreen.gameObject.SetActive(false);
        Time.timeScale = 1f;
        GameMenu.Instance.AdRewardMessage.gameObject.SetActive(true);
    }


    public void FinishGame()
    {
        gameOverStatusScreen.ShowGameOverMenu();
    }
}
