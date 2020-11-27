using UnityEngine;

public class RebornScreen : MonoBehaviour
{
    private GameOverStatusScreen gameOverStatusScreen;

    public void Constructor(GameOverStatusScreen gameOverStatusScreen)
    {
        this.gameOverStatusScreen = gameOverStatusScreen;
    }


    public void Reborn()
    {
        GameMenu.Instance.GameOverScreen.gameObject.SetActive(false);
        GameManager.Instance.SetPause(false);
        GameMenu.Instance.AdRewardMessage.gameObject.SetActive(true);

        // Возродить
        GameManager.Instance.RebornPlayer();
    }


    public void FinishGame()
    {
        gameOverStatusScreen.ShowGameOverMenu();
    }
}
