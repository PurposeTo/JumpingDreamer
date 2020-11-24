using System;
using UnityEngine;

public class GameManager : SingletonSuperMonoBehaviour<GameManager>
{
    public event Action OnGameOver;


    private bool isGameReady = false; // Игра готова? (Используется загрузчиком сцен)
    private bool isWaiting = false; // Нужно подождать? (Используется индикатором загрузки)
    // Todo: реализовать использование.
    private bool isGameActive = false; // Игра активна? (Включается при загрузки игры/возрождении, выключается при смерти)
    private bool isPause = false; // Игра на паузе? (Переключается игроком через меню паузы)
    

    public void GameOver()
    {
        Time.timeScale = 0f;
        OnGameOver?.Invoke();
    }

    /// <summary>
    /// Установить паузу игры
    /// </summary>
    public void SetPause(bool isPause)
    {
        this.isPause = isPause;
        SetTimeScale();
    }

    /// <summary>
    /// Установить значение готовности приложения. Используется загрузчиком сцен.
    /// </summary>
    public void SetGameReady(bool isGameReady)
    {
        this.isGameReady = isGameReady;
        SetTimeScale();
    }

    /// <summary>
    /// Установить значение ожидания
    /// </summary>
    public void SetWaiting(bool isWaiting)
    {
        this.isWaiting = isWaiting;
        SetTimeScale();
    }


    private void SetTimeScale()
    {
        if (isPause || !isGameReady || isWaiting) Time.timeScale = 0f;
        else Time.timeScale = 1f;
    }
}
