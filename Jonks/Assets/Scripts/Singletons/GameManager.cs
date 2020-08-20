using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    public GameObject Player;
    public PlayerPresenter PlayerPresenter { get; private set; }
    public GameObject CentreObject;
    public Centre Centre { get; private set; }

    public static bool IsGameRunning { get; private set; }
    private readonly Action<bool> ToggleGameRunningState = (gameState) => IsGameRunning = gameState;


    protected override void AwakeSingleton()
    {
        PlayerPresenter = Player.GetComponent<PlayerPresenter>();
        Centre = CentreObject.GetComponent<Centre>();

        // Изменение при смене сцены
        SceneManager.sceneLoaded += OnLoadedScene;

        // Изменение при включении GameOverMenu
        GameMenu.Instance.GameOverScreen.GameOverStatusScreen.GameOverMenu.OnToggleGameRunningState += ToggleGameRunningState;
    }


    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnLoadedScene;
        GameMenu.Instance.GameOverScreen.GameOverStatusScreen.GameOverMenu.OnToggleGameRunningState -= ToggleGameRunningState;
    }


    private void OnLoadedScene(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == SceneLoader.MainMenuName)
        {
            IsGameRunning = false;
        }
        else if (scene.name == SceneLoader.GameSceneName)
        {
            IsGameRunning = true;
        }
        else
        {
            Debug.LogWarning("Unknown scene.");
        }
    }
}
