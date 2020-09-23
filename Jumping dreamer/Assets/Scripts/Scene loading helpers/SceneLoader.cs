public static class SceneLoader
{
    public static string MainMenuName { get; } = "Main menu";
    public static string GameSceneName { get; } = "Game scene";


    public static void LoadScene(string sceneName)
    {
        Shutter.Instance.CloseShutterAndLoadScene(sceneName);
    }
}
