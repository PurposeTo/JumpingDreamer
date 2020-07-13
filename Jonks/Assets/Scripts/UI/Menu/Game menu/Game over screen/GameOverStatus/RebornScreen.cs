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

        GameMenu.Instance.GameOverScreen.gameObject.SetActive(false);
        Time.timeScale = 1f;
        GameMenu.Instance.AdRewardMessage.gameObject.SetActive(true);

        // Возродить
        if (GameManager.Instance.Player.TryGetComponent(out PlayerHealth playerHealth))
        {
            Debug.Log("Raise the player!");
            playerHealth.RaiseTheDead();
        }
        else
        {
            Debug.LogError("Can't GetComponent \"PlayerHealth\" on Player!");
        }
    }


    public void FinishGame()
    {
        gameOverStatusScreen.ShowGameOverMenu();
    }
}
