using System;
using Desdiene.Singleton;
using Desdiene.TimeControl;
using UnityEngine.SceneManagement;

public class GameManager : SingletonSuperMonoBehaviour<GameManager>
{
    public event Action OnGameOver;

     // Используется для установки паузы при смерти/возрождении игрока
    // Используется для установки паузы через UI


    protected override void AwakeSingleton()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    public void RebornPlayer()
    {
        GameMenu.Instance.GameOverScreen.GameOverStatusScreen.SetPlayerMustSeeAdTrue();
        GlobalPause.Instance.SetDeathPause(false);
        GameObjectsHolder.Instance.PlayerPresenter.PlayerHealth.RaiseTheDead();
    }


    public void GameOver()
    {
        GlobalPause.Instance.SetDeathPause(true);
        OnGameOver?.Invoke();
    }

    /// <summary>
    /// Установить паузу игры
    /// </summary>
    public void SetPlayerPause(bool isPause)
    {
        GlobalPause.Instance.SetPlayerPause(isPause);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GlobalPause.Instance.SetPlayerPause(false);
        GlobalPause.Instance.SetDeathPause(false);
    }
}
