using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleSceneLoader : SingletonSuperMonoBehaviour<SingleSceneLoader>
{
    public const string MainMenuName = "Main menu";
    public const string GameSceneName = "Game scene";

    private string sceneToLoadName;

    private Pauser pauser;

    protected override void AwakeSingleton()
    {
        pauser = new Pauser(this);

        SceneManager.sceneLoaded += OnSceneLoaded;

        Shutter.InitializedInstance += (instance) =>
        {
            instance.OnShutterOpen += OnShutterOpen;
            instance.OnShutterClose += OnShutterClose;

            // Остановить время при старте игры и ждать выполнения метода OpenShutter()
            pauser.SetPause(true);
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
        pauser.SetPause(true);
        Shutter.Instance.CloseShutter();
    }


    private void OnShutterClose()
    {
        SceneManager.LoadScene(sceneToLoadName);
        sceneToLoadName = ""; // Необходимо очистить поле после загрузки сцены
    }


    private void OnShutterOpen()
    {
        pauser.SetPause(false);
    }
}
