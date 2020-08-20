using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;

public class GPGSAuthentication : SingletonMonoBehaviour<GPGSAuthentication>
{
    public static PlayGamesPlatform platform;
    public TextMeshProUGUI AuthenticateStatus;

    public static bool IsAuthenticated
    {
        get
        {
            if (platform != null) { return platform.IsAuthenticated(); }
            return false;
        }
    }


    protected override void AwakeSingleton()
    {
        if (platform != null)
        {
            Debug.LogError("PlayGamesPlatform.Activate() is already activated!");
        }

        PlayGamesClientConfiguration configuration = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();

        PlayGamesPlatform.InitializeInstance(configuration);
        PlayGamesPlatform.DebugLogEnabled = true;

        platform = PlayGamesPlatform.Activate();

        // Аутентификация пользователя
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (result) =>
        {
            Debug.Log($"Authenticate is completed with code: {result}");
            AuthenticateStatus.text = $"{result}";
        });

        //Social.Active.localUser.Authenticate(success =>
        //{
        //    if (success)
        //    {
        //        Debug.Log("Authenticate is successfully!");
        //    }
        //    else
        //    {
        //        Debug.LogWarning("Failed to authenticate!");
        //    }
        //});
    }


    private void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit!");
        platform.SignOut();
    }
}
