using System;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using Debug = UnityEngine.Debug;
using System.Collections;

public class GoogleAdMobController : SingletonMonoBehaviour<GoogleAdMobController>
{
    private readonly string rewardedVideoAdForTest_ID = "ca-app-pub-3940256099942544/5224354917";

    private RewardedAd rewardedAd;

    public Action OnAdLoaded;
    public Action OnAdFailedToLoad;
    public Action OnAdOpening;
    public Action OnAdFailedToShow;
    public Action OnUserEarnedReward;
    public Action OnAdClosed;

    private readonly InternetConnectionChecker connectionChecker = new InternetConnectionChecker();
    private Coroutine TryToReLoadAdRoutine = null;


    protected override void AwakeSingleton()
    {
        rewardedAd = CreateAndLoadRewardedAd(rewardedVideoAdForTest_ID);
    }


    public void OnDestroy()
    {
        UnsubscribeEvents(rewardedAd);
    }


    public void ShowRewardVideoAd(Action<bool> isAdWasReallyLoadedCallback, Action<bool> hasAdBeenShowed)
    {
        bool isAdWasReallyLoaded = rewardedAd.IsLoaded();
        isAdWasReallyLoadedCallback?.Invoke(isAdWasReallyLoaded);

        Debug.Log($"Try to show Ad. isAdLoaded = {isAdWasReallyLoaded}");

        if (isAdWasReallyLoaded)
        {
            int operationTimeOut = 4;

            StartCoroutine(connectionChecker.PingGoogleEnumerator(isInternetAvaliable =>
            {
                if (isInternetAvaliable)
                {
                    Debug.Log($"rewardedAd.Show() call");
                    rewardedAd.Show();
                }

                hasAdBeenShowed?.Invoke(isInternetAvaliable);
            },
            timeOut: operationTimeOut));
        }
        else hasAdBeenShowed?.Invoke(isAdWasReallyLoaded);
    }


    public RewardedAd CreateAndLoadRewardedAd(string adUnitId)
    {
        RewardedAd rewardedAd = new RewardedAd(adUnitId);

        // Called when an ad request has successfully loaded.
        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;

        LoadRewardedAd(rewardedAd);
        return rewardedAd;
    }


    private void UnsubscribeEvents(RewardedAd rewardedAd)
    {
        // Called when an ad request has successfully loaded.
        rewardedAd.OnAdLoaded -= HandleRewardedAdLoaded;
        // Called when an ad request failed to load.
        rewardedAd.OnAdFailedToLoad -= HandleRewardedAdFailedToLoad;
        // Called when an ad is shown.
        rewardedAd.OnAdOpening -= HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        rewardedAd.OnAdFailedToShow -= HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        rewardedAd.OnUserEarnedReward -= HandleUserEarnedReward;
        // Called when the ad is closed.
        rewardedAd.OnAdClosed -= HandleRewardedAdClosed;
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


    private IEnumerator TryToReLoadAdEnumerator()
    {
        yield return new WaitForSecondsRealtime(30f);

        // Не учитывает реальный доступ к сети. Учитывает только подключение.
        bool isInternetEnabled = Application.internetReachability != NetworkReachability.NotReachable;

        Debug.Log($"Try to reload Ad: isInternetEnabled = {isInternetEnabled}.");

        if (isInternetEnabled) LoadRewardedAd(rewardedAd);

        TryToReLoadAdRoutine = null;
    }


    private void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdLoaded event received");
        OnAdLoaded?.Invoke();
    }

    private void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        Debug.Log($"HandleRewardedAdFailedToLoad event received with message: {args.Message}. " +
            $"And now rewardedAd.IsLoaded() is {rewardedAd.IsLoaded()}");

        if (TryToReLoadAdRoutine == null) TryToReLoadAdRoutine = StartCoroutine(TryToReLoadAdEnumerator());

        OnAdFailedToLoad?.Invoke();
    }

    private void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdOpening event received");
        OnAdOpening?.Invoke();
    }

    private void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        Debug.Log($"HandleRewardedAdFailedToShow event received with message: {args.Message}");
        LoadRewardedAd(rewardedAd);
        OnAdFailedToShow?.Invoke();
    }

    private void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdClosed event received");
        LoadRewardedAd(rewardedAd);
        OnAdClosed?.Invoke();
    }

    private void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        Debug.Log($"HandleRewardedAdRewarded event received for {amount} {type}");
        OnUserEarnedReward?.Invoke();
    }
}