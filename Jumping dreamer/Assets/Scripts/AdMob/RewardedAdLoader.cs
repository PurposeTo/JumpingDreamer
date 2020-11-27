using UnityEngine;
using GoogleMobileAds.Api;
using System;
using System.Collections;

public class RewardedAdLoader : IRewardedAdLoader
{
    private readonly SuperMonoBehaviour superMonoBehaviour;
    private readonly CommandQueueMainThreadExecutor commandQueueHandler;

    public RewardedAdLoader(SuperMonoBehaviour superMonoBehaviour, CommandQueueMainThreadExecutor commandQueueHandler)
    {
        this.commandQueueHandler = commandQueueHandler != null ? commandQueueHandler : throw new ArgumentNullException(nameof(commandQueueHandler));
        this.superMonoBehaviour = superMonoBehaviour != null ? superMonoBehaviour : throw new ArgumentNullException(nameof(superMonoBehaviour));

        tryToReLoadAdInfo = superMonoBehaviour.CreateCoroutineContainer();
        CreateNewRewardedAd();
    }


    private readonly string rewardedVideoAdForTest_ID = "ca-app-pub-3940256099942544/5224354917";

    private RewardedAd rewardedAd;

    private ICoroutineContainer tryToReLoadAdInfo;

    public event Action OnAdOpening;
    public event Action OnAdFailedToShow;
    public event Action OnUserEarnedReward;
    public event Action OnAdClosed;

    public bool IsAdLoaded()
    {
        if (rewardedAd != null) return rewardedAd.IsLoaded();
        else return false;
    }


    public void CreateNewRewardedAd()
    {
        if (rewardedAd != null) UnsubscribeRewardedAdEvents(rewardedAd);

        rewardedAd = GetAndLoadRewardedAd();
    }


    public void Show()
    {
        rewardedAd.Show();
    }


    private void SubscribeRewardedAdEvents(RewardedAd rewardedAd)
    {
        // Called when an ad request failed to load.
        rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        // Called when an ad request has successfully loaded.
        rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
        // Called when an ad is shown.
        rewardedAd.OnAdOpening += HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
        // Called when the ad is closed.
        rewardedAd.OnAdClosed += HandleRewardedAdClosed;
    }


    private void UnsubscribeRewardedAdEvents(RewardedAd rewardedAd)
    {
        // Called when an ad request failed to load.
        rewardedAd.OnAdFailedToLoad -= HandleRewardedAdFailedToLoad;
        // Called when an ad request has successfully loaded.
        rewardedAd.OnAdLoaded -= HandleRewardedAdLoaded;
        // Called when an ad is shown.
        rewardedAd.OnAdOpening -= HandleRewardedAdOpening;
        // Called when an ad request failed to show.
        rewardedAd.OnAdFailedToShow -= HandleRewardedAdFailedToShow;
        // Called when the user should be rewarded for interacting with the ad.
        rewardedAd.OnUserEarnedReward -= HandleUserEarnedReward;
        // Called when the ad is closed.
        rewardedAd.OnAdClosed -= HandleRewardedAdClosed;
    }


    private RewardedAd GetAndLoadRewardedAd()
    {
        RewardedAd rewardedAd = new RewardedAd(rewardedVideoAdForTest_ID);

        SubscribeRewardedAdEvents(rewardedAd); // Подписаться на эвенты загрузки необходимо до отправки запроса на загрузку рекламы
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


    private void TryToReLoadAd() => superMonoBehaviour.ExecuteCoroutineContinuously(ref tryToReLoadAdInfo, TryToReLoadAdEnumerator());


    private IEnumerator TryToReLoadAdEnumerator()
    {
        Debug.Log($"TryToReLoadAdEnumerator started. rewardedAd.IsLoaded() = {IsAdLoaded()}");

        while (!IsAdLoaded())
        {
            Debug.Log($"TryToReLoadAdEnumerator before WaitForSecondsRealtime.");
            yield return new WaitForSecondsRealtime(30f);

            // Не учитывает реальный доступ к сети. Учитывает только подключение.
            bool isInternetEnabled = Application.internetReachability != NetworkReachability.NotReachable;

            Debug.Log($"Try to reload Ad: isInternetEnabled = {isInternetEnabled}.");

            if (isInternetEnabled) CreateNewRewardedAd();
        }
    }


    #region event calls not from the main thread

    private void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdLoaded event received");
    }


    private void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        Debug.Log($"HandleRewardedAdFailedToLoad event received with message: {args.Message}. " +
            $"And now rewardedAd.IsLoaded() is {IsAdLoaded()}");

        commandQueueHandler.SetCommandToQueue(() => TryToReLoadAd());
    }


    private void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardedAdOpening event received");

        commandQueueHandler.SetCommandToQueue(() => OnAdOpening?.Invoke());
    }

    private void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        Debug.Log($"HandleRewardedAdFailedToShow event received with message: {args.Message}");

        commandQueueHandler.SetCommandToQueue(() => OnAdFailedToShow?.Invoke());

        commandQueueHandler.SetCommandToQueue(() => CreateNewRewardedAd());
    }

    private void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        Debug.Log($"HandleRewardedAdClosed event received.");

        commandQueueHandler.SetCommandToQueue(() => OnAdClosed?.Invoke());

        commandQueueHandler.SetCommandToQueue(() => CreateNewRewardedAd());
    }

    private void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
        Debug.Log($"HandleRewardedAdRewarded event received for {amount} {type}");

        commandQueueHandler.SetCommandToQueue(() => OnUserEarnedReward?.Invoke());
    }

    #endregion
}


public interface IRewardedAdLoader
{
    event Action OnAdOpening;
    event Action OnAdFailedToShow;
    event Action OnUserEarnedReward;
    event Action OnAdClosed;

    bool IsAdLoaded();

    void Show();
}
