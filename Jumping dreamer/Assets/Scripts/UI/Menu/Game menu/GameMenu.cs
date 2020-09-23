using UnityEngine;

public class GameMenu : SingletonMonoBehaviour<GameMenu>
{
    public GameOverScreen GameOverScreen;
    public PlayerUI PlayerUI;
    public PauseMenu PauseMenu;
    public AdRewardMessage AdRewardMessage;


    public void GameOver()
    {
        Time.timeScale = 0f;
        GameOverScreen.gameObject.SetActive(true);
    }
}
