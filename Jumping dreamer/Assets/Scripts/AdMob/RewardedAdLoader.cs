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
        RewardedAd = GetAndLoadRewardedAd(BeforeLoadRewardedAd, rewardedVideoAdForTest_ID);
    }


    /// <summary>
    /// Рекламу, которая была просмотрена, невозможно перезагрузить. 
    /// Для этого необходимо создать новый экз. рекламы.
    /// </summary>
    /// <param name="BeforeLoadRewardedAd">Используйте этот Action для подписания на события RewardedAd</param>
    /// <param name="adUnitId"></param>
    /// <returns></returns>
    private RewardedAd GetAndLoadRewardedAd(Action BeforeLoadRewardedAd, string adUnitId)
    {
        RewardedAd rewardedAd = new RewardedAd(adUnitId);
        BeforeLoadRewardedAd?.Invoke();

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
}
