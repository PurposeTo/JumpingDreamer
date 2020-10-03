using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;

public class GPGSAuthentication : SingletonMonoBehaviour<GPGSAuthentication>
{
    public static PlayGamesPlatform Platform { get; private set; }
    public TextMeshProUGUI AuthenticateStatus;

    public static bool IsAuthenticated
    {
        get
        {
            if (Platform != null) { return Platform.IsAuthenticated(); }
            return false;
        }
    }


    protected override void AwakeSingleton()
    {
        ConfigurePlayGamesPlatform();
        Authenticate();
    }


    public void Authenticate()
    {
        if (IsAuthenticated)
        {
            Debug.LogError("Authentication has already been passed!");
            return;
        }

        // Аутентификация пользователя
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (result) =>
        {
            Debug.Log($"Authenticate is completed with code: {result}");
            if (AuthenticateStatus != null) AuthenticateStatus.text = $"{result}";
        });
    }


    private void ConfigurePlayGamesPlatform()
    {
        if (Platform != null)
        {
            Debug.LogError("PlayGamesPlatform.Activate() is already activated!");
        }

        PlayGamesClientConfiguration configuration = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();

        PlayGamesPlatform.InitializeInstance(configuration);
        PlayGamesPlatform.DebugLogEnabled = true;

        Platform = PlayGamesPlatform.Activate();
    }


    private void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit!");
        Platform.SignOut();
    }
}
