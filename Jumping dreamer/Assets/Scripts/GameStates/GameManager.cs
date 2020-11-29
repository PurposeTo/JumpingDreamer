using System;
using UnityEngine.SceneManagement;

public class GameManager : SingletonSuperMonoBehaviour<GameManager>
{
    public event Action OnGameOver;

    private Pauser gamePauser; // Используется для установки паузы при смерти/возрождении игрока
    private Pauser gameUIPauser; // Используется для установки паузы через UI


    protected override void AwakeSingleton()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        gameUIPauser = new Pauser(this);
        gamePauser = new Pauser(this);
    }


    public void RebornPlayer()
    {
        GameMenu.Instance.GameOverScreen.GameOverStatusScreen.SetPlayerMustSeeAdTrue();
        gamePauser.SetPause(false);
        GameObjectsHolder.Instance.PlayerPresenter.PlayerHealth.RaiseTheDead();
    }


    public void GameOver()
    {
        gamePauser.SetPause(true);
        OnGameOver?.Invoke();
    }

    /// <summary>
    /// Установить паузу игры
    /// </summary>
    public void SetPause(bool isPause)
    {
        gameUIPauser.SetPause(isPause);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gamePauser.SetPause(false);
        gameUIPauser.SetPause(false);
    }
}
