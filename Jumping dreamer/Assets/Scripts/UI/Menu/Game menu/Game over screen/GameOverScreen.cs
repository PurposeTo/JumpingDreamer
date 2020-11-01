using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    public GameOverStatusScreen GameOverStatusScreen;
    public EarnedRewardsInGame EarnedRewardsInGame;


    private void OnEnable()
    {
        SetGameUIActive(false);
    }


    private void OnDisable()
    {
        SetGameUIActive(true);
    }


    private void SetGameUIActive(bool isActive)
    {
        GameMenu.Instance.PlayerUI.gameObject.SetActive(isActive);
        GameMenu.Instance.PauseMenu.PauseButton.SetActive(isActive);
    }
}
