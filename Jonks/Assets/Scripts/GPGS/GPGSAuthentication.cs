using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using TMPro;

public class GPGSAuthentication : SingletonMonoBehaviour<GPGSAuthentication>
{
    public static PlayGamesPlatform platform;
    public TextMeshProUGUI AuthenticateStatus;


    protected override void AwakeSingleton()
    {
        if (platform != null)
        {
            Debug.LogError("PlayGamesPlatform.Activate() is already activated!");
        }

        PlayGamesClientConfiguration configuration = new PlayGamesClientConfiguration.Builder().Build();

        PlayGamesPlatform.InitializeInstance(configuration);
        PlayGamesPlatform.DebugLogEnabled = true;

        platform = PlayGamesPlatform.Activate();

        // authenticate user:
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
