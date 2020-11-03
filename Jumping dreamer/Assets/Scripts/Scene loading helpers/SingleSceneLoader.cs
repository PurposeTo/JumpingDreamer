public static class SingleSceneLoader
{
    public static string MainMenuName { get; } = "Main menu";
    public static string GameSceneName { get; } = "Game scene";


    public static void LoadScene(string sceneName)
    {
        Shutter.Instance.CloseShutterAndLoadScene(sceneName);
        GameManager.Instance.SetPause(false); // Автоматически снимать игру с паузы при перезагрузке сцены
    }
}
