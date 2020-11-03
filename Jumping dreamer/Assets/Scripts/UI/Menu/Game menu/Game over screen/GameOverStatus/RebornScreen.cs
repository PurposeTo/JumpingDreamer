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
        gameOverStatusScreen.SetPlayerMustSeeAdTrue();

        GameMenu.Instance.GameOverScreen.gameObject.SetActive(false);
        GameManager.Instance.SetPause(false);
        GameMenu.Instance.AdRewardMessage.gameObject.SetActive(true);

        // Возродить
        GameObjectsHolder.Instance.PlayerPresenter.PlayerHealth.RaiseTheDead();
    }


    public void FinishGame()
    {
        gameOverStatusScreen.ShowGameOverMenu();
    }
}
