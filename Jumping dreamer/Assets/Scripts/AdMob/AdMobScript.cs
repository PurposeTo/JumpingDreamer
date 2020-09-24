using System;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdMobScript : SingletonMonoBehaviour<AdMobScript>
{
    private const string ADS_ID = "ca-app-pub-8365272256827287~9135876659";
    private const string rewardedVideoAd_ID = "ca-app-pub-8365272256827287/5171106131";
    private const string rewardedVideoAdForTest_ID = "ca-app-pub-3940256099942544/5224354917";

    private RewardBasedVideoAd rewardBasedVideoAd;


    protected override void AwakeSingleton()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });

        // Get singleton reward based video ad reference.
        rewardBasedVideoAd = RewardBasedVideoAd.Instance;

        // Called when an ad request has successfully loaded.
        rewardBasedVideoAd.OnAdLoaded += HandleRewardBasedVideoLoaded;
        // Called when an ad request failed to load.
        rewardBasedVideoAd.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        // Called when an ad is shown.
        rewardBasedVideoAd.OnAdOpening += HandleRewardBasedVideoOpened;
        // Called when the ad starts to play.
        rewardBasedVideoAd.OnAdStarted += HandleRewardBasedVideoStarted;
        // Called when the user should be rewarded for watching a video.
        rewardBasedVideoAd.OnAdRewarded += HandleRewardBasedVideoRewarded;
        // Called when the ad is closed.
        rewardBasedVideoAd.OnAdClosed += HandleRewardBasedVideoClosed;
        // Called when the ad click caused the user to leave the application.
        rewardBasedVideoAd.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;

        RequestRewardBasedVideo();
    }


    private void OnDestroy()
    {
        rewardBasedVideoAd.OnAdLoaded -= HandleRewardBasedVideoLoaded;
        rewardBasedVideoAd.OnAdFailedToLoad -= HandleRewardBasedVideoFailedToLoad;
        rewardBasedVideoAd.OnAdOpening -= HandleRewardBasedVideoOpened;
        rewardBasedVideoAd.OnAdStarted -= HandleRewardBasedVideoStarted;
        rewardBasedVideoAd.OnAdRewarded -= HandleRewardBasedVideoRewarded;
        rewardBasedVideoAd.OnAdClosed -= HandleRewardBasedVideoClosed;
        rewardBasedVideoAd.OnAdLeavingApplication -= HandleRewardBasedVideoLeftApplication;
    }


    private void RequestRewardBasedVideo()
    {
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded video ad with the request.
        rewardBasedVideoAd.LoadAd(request, rewardedVideoAdForTest_ID);
    }


    public void ShowRewardVideoAd(Action<bool> callback)
    {
        bool isAdLoaded = rewardBasedVideoAd.IsLoaded();
        if (isAdLoaded) rewardBasedVideoAd.Show();
        callback(isAdLoaded);
    }



    #region event calls not from the main thread
    private void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardBasedVideoLoaded event received");
    }


    private void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log($"HandleRewardBasedVideoFailedToLoad event received with message: {args.Message}");
    }


    private void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardBasedVideoOpened event received");
    }


    private void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardBasedVideoStarted event received");
    }


    private void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardBasedVideoClosed event received");
        // Перезагрузить рекламу
        RequestRewardBasedVideo();
    }


    private void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        Debug.Log($"HandleRewardBasedVideoRewarded event received for {amount} {type}");
    }


    private void HandleRewardBasedVideoLeftApplication(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardBasedVideoLeftApplication event received");
    }
    #endregion
}
