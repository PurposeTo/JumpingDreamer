using UnityEngine;

public class GameMenu : Singleton<GameMenu>
{
    public GameOverScreen GameOverScreen;
    public PlayerUI PlayerUI;
    public PauseMenu PauseMenu;


    public void GameOver()
    {
        Time.timeScale = 0f;
        PlayerUI.gameObject.SetActive(false);
        GameOverScreen.gameObject.SetActive(true);
    }
}
