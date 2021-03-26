using Desdiene.Singleton;
using UnityEngine;

public class GameMenu : SingletonSuperMonoBehaviour<GameMenu>
{
    public GameOverScreen GameOverScreen;
    public PlayerUI PlayerUI;
    public PauseMenu PauseMenu;
    public AdRewardMessage AdRewardMessage;


    protected override void AwakeSingleton()
    {
        GameManager.Instance.OnGameOver += EnableGameOverScreen;
    }


    private void OnDestroy()
    {
        GameManager.Instance.OnGameOver -= EnableGameOverScreen;
    }


    private void EnableGameOverScreen()
    {
        GameOverScreen.gameObject.SetActive(true);
    }
}
