using System;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using Debug = UnityEngine.Debug;
using System.Collections;

[RequireComponent(typeof(CommandQueueHandler))]
public class GoogleAdMobController : SingletonMonoBehaviour<GoogleAdMobController>
{
    private readonly string rewardedVideoAdForTest_ID = "ca-app-pub-3940256099942544/5224354917";

    private RewardedAd rewardedAd;

    public Action OnAdLoaded;
    public Action OnAdFailedToLoad;
    public Action OnAdOpening;
    public Action OnAdFailedToShow;
    public Action OnUserEarnedReward;
    public Action<bool> OnAdClosed;

    private bool mustRewardPlayer = false; // Кеширую награду, что бы можно было по событию OnAdClosed узнать, был ли награжден игрок.

    private CommandQueueHandler commandQueueHandler;
    private readonly InternetConnectionChecker connectionChecker = new InternetConnectionChecker();
    private Coroutine TryToReLoadAdRoutine = null;

    protected override void AwakeSingleton()
    {
        commandQueueHandler = gameObject.GetComponent<CommandQueueHandler>();
        rewardedAd = CreateAndLoadRewardedAd(rewardedVideoAdForTest_ID);
    }


    private void OnDestroy()
    {
        UnsubscribeEvents(rewardedAd);
    }


    public bool IsAdWasLoaded()
    {
        return rewardedAd.IsLoaded();
    }


    public void ShowRewardVideoAd(Action<bool> isAdWasReallyLoadedCallback)
    {
        mustRewardPlayer = false; // Обнуляю значение награды

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
                else
                {
                    Debug.Log($"Force invoke OnAdFailedToShow because internet is not avaliable.");
                    OnAdFailedToShow?.Invoke();
                }
            },
            timeOut: operationTimeOut));
        }
    }


    public RewardedAd CreateAndLoadRewardedAd(string adUnitId)
    {
        RewardedAd rewardedAd = new RewardedAd(adUnitId);
        SubscribeEvents(rewardedAd);

        LoadRewardedAd(rewardedAd);
        return rewardedAd;
    }


    private void SubscribeEvents(RewardedAd rewardedAd)
    {
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

    #region event calls not from the main thread

    private void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdLoaded event received");
        commandQueueHandler.SetCommandToQueue(() => OnAdLoaded?.Invoke());
    }

    private void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        Debug.Log($"HandleRewardedAdFailedToLoad event received with message: {args.Message}. " +
            $"And now rewardedAd.IsLoaded() is {rewardedAd.IsLoaded()}");


        void TryToReLoadAd()
        {
            if (TryToReLoadAdRoutine == null) TryToReLoadAdRoutine = StartCoroutine(TryToReLoadAdEnumerator());
        };

        commandQueueHandler.SetCommandToQueue(TryToReLoadAd);

        commandQueueHandler.SetCommandToQueue(() => OnAdFailedToLoad?.Invoke());
    }

    private void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdOpening event received");

        commandQueueHandler.SetCommandToQueue(() => OnAdOpening?.Invoke());
    }

    private void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        Debug.Log($"HandleRewardedAdFailedToShow event received with message: {args.Message}");

        commandQueueHandler.SetCommandToQueue(() => LoadRewardedAd(rewardedAd));

        commandQueueHandler.SetCommandToQueue(() => OnAdFailedToShow?.Invoke());
    }

    private void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdClosed event received");

        commandQueueHandler.SetCommandToQueue(() => LoadRewardedAd(rewardedAd));

        commandQueueHandler.SetCommandToQueue(() => OnAdClosed?.Invoke(mustRewardPlayer));

        mustRewardPlayer = false; // Обнуляю значение награды.
    }

    private void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        Debug.Log($"HandleRewardedAdRewarded event received for {amount} {type}");

        mustRewardPlayer = true; // Игрок был награжден!

        commandQueueHandler.SetCommandToQueue(() => OnUserEarnedReward?.Invoke());
    }
    #endregion
}