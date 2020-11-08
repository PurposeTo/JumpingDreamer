using System;
using UnityEngine;

public class GameManager : SingletonSuperMonoBehaviour<GameManager>
{
    public event Action OnGameOver;


    private bool isPause = false;
    private bool isGameReady = false;


    public void GameOver()
    {
        Time.timeScale = 0f;
        OnGameOver?.Invoke();
    }


    public void SetPause(bool isPause)
    {
        this.isPause = isPause;
        SetTimeScale();
    }


    public void SetGameReady(bool isGameReady)
    {
        this.isGameReady = isGameReady;
        SetTimeScale();
    }


    private void SetTimeScale()
    {
        if (isPause || !isGameReady) Time.timeScale = 0f;
        else Time.timeScale = 1f;
    }
}
