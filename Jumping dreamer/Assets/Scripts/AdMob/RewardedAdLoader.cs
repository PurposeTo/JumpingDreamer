using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class RewardedAdLoader
{
    private readonly CommandQueueMainThreadExecutor commandQueueHandler;

    public RewardedAdLoader(CommandQueueMainThreadExecutor commandQueueHandler)
    {
        this.commandQueueHandler = commandQueueHandler ?? throw new ArgumentNullException(nameof(commandQueueHandler));
    }


    private readonly string rewardedVideoAdForTest_ID = "ca-app-pub-3940256099942544/5224354917";

    public RewardedAd RewardedAd { get; private set; }

    public event Action OnAdLoaded;



    public bool IsAdLoaded()
    {
        if (RewardedAd != null) return RewardedAd.IsLoaded();
        else return false;
    }


    public void CreateRewardedAd()
    {
        if (RewardedAd != null) UnsubscribeLoadingEvents(RewardedAd);

        RewardedAd = GetAndLoadRewardedAd();
    }


    private RewardedAd GetAndLoadRewardedAd()
    {
        RewardedAd rewardedAd = new RewardedAd(rewardedVideoAdForTest_ID);

        SubscribeLoadingEvents(rewardedAd); // Подписаться на эвенты загрузки необходимо до отправки запроса на загрузку рекламы
        LoadRewardedAd(rewardedAd);

        return rewardedAd;
    }


    private void LoadRewardedAd(RewardedAd rewardedAd)
    {
        bool isAdLoaded = rewardedAd.IsLoaded();

        string AdLoadingResultLog;

        if (!isAdLoaded)
        {
            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the rewarded ad with the request.
            rewardedAd.LoadAd(request);

            AdLoadingResultLog = "Loading Ad...";
        }
        else AdLoadingResultLog = "Ad is already loaded!";

        Debug.Log($"You try to load Ad. isAdLoaded = {isAdLoaded}. {AdLoadingResultLog}");
    }


    private void SubscribeLoadingEvents(RewardedAd rewardedAd)
    {
        // Called when an ad request has successfully loaded.
        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
    }


    private void UnsubscribeLoadingEvents(RewardedAd rewardedAd)
    {
        // Called when an ad request has successfully loaded.
        rewardedAd.OnAdLoaded -= HandleRewardedAdLoaded;
    }


    #region event calls not from the main thread

    private void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdLoaded event received");
        commandQueueHandler.SetCommandToQueue(() => OnAdLoaded?.Invoke());
    }
    #endregion
}
