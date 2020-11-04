using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class RewardedAdLoader
{
    private readonly string rewardedVideoAdForTest_ID = "ca-app-pub-3940256099942544/5224354917";


    public RewardedAd RewardedAd { get; private set; }


    public bool IsAdWasLoaded()
    {
        if (RewardedAd != null) return RewardedAd.IsLoaded();
        else return false;
    }


    /// <summary>
    /// Создать новую рекламу
    /// </summary>
    /// <param name="BeforeLoadRewardedAd">Используйте этот Action для подписания на события RewardedAd</param>
    public void CreateRewardedAd(Action BeforeLoadRewardedAd)
    {
        RewardedAd = new RewardedAd(rewardedVideoAdForTest_ID);

        // Подписчики события используют this.RewardedAd, поэтому поле должно быть УЖЕ инициализированно.
        BeforeLoadRewardedAd?.Invoke();
        LoadRewardedAd(RewardedAd);
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
}
