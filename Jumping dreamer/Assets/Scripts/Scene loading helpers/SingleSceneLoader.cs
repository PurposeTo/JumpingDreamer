using UnityEngine;
using UnityEngine.SceneManagement;
using Desdiene.Time_control;
using Desdiene.Singleton;

public class SingleSceneLoader : SingletonSuperMonoBehaviour<SingleSceneLoader>
{
    public const string MainMenuName = "Main menu";
    public const string GameSceneName = "Game scene";

    private string sceneToLoadName;

    protected override void AwakeSingleton()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        Shutter.InitializedInstance += (instance) =>
        {
            instance.OnShutterOpen += OnShutterOpen;
            instance.OnShutterClose += OnShutterClose;

            // Остановить время при старте игры и ждать выполнения метода OpenShutter()
            GlobalPause.Instance.SetSceneLoading(true);
        };
    }


    protected override void OnDestroyWrapped()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Shutter.Instance.OnShutterOpen -= OnShutterOpen;
        Shutter.Instance.OnShutterClose -= OnShutterClose;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"OnSceneLoaded: {scene.name} in mode: {mode}");

        Shutter.Instance.OpenShutter();
    }


    public void LoadScene(string sceneName)
    {
        sceneToLoadName = sceneName;
        GlobalPause.Instance.SetSceneLoading(true);
        Shutter.Instance.CloseShutter();
    }


    private void OnShutterClose()
    {
        SceneManager.LoadScene(sceneToLoadName);
        sceneToLoadName = ""; // Необходимо очистить поле после загрузки сцены
    }


    private void OnShutterOpen()
    {
        GlobalPause.Instance.SetSceneLoading(false);
    }
}
