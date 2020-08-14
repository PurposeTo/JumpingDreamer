using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    public GameOverStatusScreen GameOverStatusScreen;


    private void OnEnable()
    {
        GameMenu.Instance.PlayerUI.gameObject.SetActive(false);
        GameMenu.Instance.PauseMenu.PauseButton.SetActive(false);
    }


    private void OnDisable()
    {
        GameMenu.Instance.PlayerUI.gameObject.SetActive(true);
        GameMenu.Instance.PauseMenu.PauseButton.SetActive(true);
    }
}
