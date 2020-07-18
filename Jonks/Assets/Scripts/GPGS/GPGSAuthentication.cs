using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GPGSAuthentication : MonoBehaviour
{
    public static PlayGamesPlatform platform;

    private void Awake()
    {
        if (platform == null)
        {
            PlayGamesClientConfiguration configuration = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(configuration);
            PlayGamesPlatform.DebugLogEnabled = true;

            platform = PlayGamesPlatform.Activate();

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Social.Active.localUser.Authenticate(success =>
        {
            if (success)
            {
                Debug.Log("Authenticate is successfully!");
            }
            else
            {
                Debug.LogWarning("Failed to authenticate!");
            }
        });
    }


    private void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit!");
        platform.SignOut();
    }
}
