using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;
using Desdiene.Singleton;

public class GPGSAuthentication : SingletonSuperMonoBehaviour<GPGSAuthentication>
{
    public static PlayGamesPlatform Platform { get; private set; }
    public SignInStatus SignInStatus { get; private set; } = SignInStatus.NotAuthenticated;
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
        else
        {
            Platform.SignOut();
            Debug.Log($"GPGS Sign out have performed");
        }

        // Аутентификация пользователя
        Platform.Authenticate(SignInInteractivity.CanPromptOnce, (result) =>
        {
            Debug.Log($"Authenticate is completed with code: {result}");
            SignInStatus = result;

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
