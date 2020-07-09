using UnityEngine;
using GoogleMobileAds.Api;


public class CollectRewardsScreen : MonoBehaviour
{
    private GameOverStatusScreen gameOverStatusScreen;


    private void Start()
    {
        // Called when the user should be rewarded for watching a video.
        RewardBasedVideoAd.Instance.OnAdRewarded += HandleRewardBasedVideoRewarded;
    }


    private void OnDestroy()
    {
        RewardBasedVideoAd.Instance.OnAdRewarded -= HandleRewardBasedVideoRewarded;
    }


    public void Initialize(GameOverStatusScreen gameOverStatusScreen)
    {
        this.gameOverStatusScreen = gameOverStatusScreen;
    }


    public void CollectRewards()
    {
        // Показать рекламу

        if (RewardBasedVideoAd.Instance.IsLoaded())
        {
            RewardBasedVideoAd.Instance.Show();
        }
        else
        {
            Debug.LogError("Ad was loaded, but now it isn't");
            gameOverStatusScreen.ShowGameOverMenu();
        }
    }

    public void OpenMainMenu()
    {
        SceneLoader.LoadScene(SceneLoader.MainMenuName);
    }


    private void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        //Если игрок посмотрел рекламу, наградить его
        gameOverStatusScreen.ShowGameOverMenu();
    }
}
