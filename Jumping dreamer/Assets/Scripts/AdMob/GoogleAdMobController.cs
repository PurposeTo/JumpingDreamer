using System;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using Debug = UnityEngine.Debug;
using System.Collections;

[RequireComponent(typeof(CommandQueueMainThreadExecutor))]
public class GoogleAdMobController : SingletonMonoBehaviour<GoogleAdMobController>
{

    private readonly RewardedAdLoader rewardedAdLoader = new RewardedAdLoader();

    public event Action OnAdLoaded;
    public event Action OnAdFailedToLoad;
    public event Action OnAdOpening;
    public event Action OnAdFailedToShow;
    public event Action OnUserEarnedReward;
    public event Action<bool> OnAdClosed;

    private CommandQueueMainThreadExecutor commandQueueHandler;
    private readonly InternetConnectionChecker connectionChecker = new InternetConnectionChecker();
    private Coroutine TryToReLoadAdRoutine = null;

    private bool mustRewardPlayer = false;

    protected override void AwakeSingleton()
    {
        commandQueueHandler = gameObject.GetComponent<CommandQueueMainThreadExecutor>();
        CreateRewardedAd();
    }


    private void OnDestroy()
    {
        if (rewardedAdLoader.RewardedAd != null) UnsubscribeEvents(rewardedAdLoader.RewardedAd);
    }


    private void CreateRewardedAd()
    {
        rewardedAdLoader.CreateRewardedAd(() => SubscribeEvents(rewardedAdLoader.RewardedAd));
    }


    private void ReCreateRewardedAd()
    {
        UnsubscribeEvents(rewardedAdLoader.RewardedAd);
        CreateRewardedAd();
    }


    public bool IsAdWasLoaded()
    {
        return rewardedAdLoader.IsAdWasLoaded();
    }


    public void ShowRewardVideoAd(Action<bool> isAdWasReallyLoadedCallback)
    {
        mustRewardPlayer = false; // Обнуляю значение награды

        bool isAdWasReallyLoaded = rewardedAdLoader.IsAdWasLoaded();
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
                    rewardedAdLoader.RewardedAd.Show();
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


    private IEnumerator TryToReLoadAdEnumerator()
    {
        Debug.Log($"TryToReLoadAdEnumerator started. rewardedAd.IsLoaded() = {rewardedAdLoader.IsAdWasLoaded()}");

        while (!rewardedAdLoader.IsAdWasLoaded())
        {
            Debug.Log($"TryToReLoadAdEnumerator before WaitForSecondsRealtime.");
            yield return new WaitForSecondsRealtime(30f);

            // Не учитывает реальный доступ к сети. Учитывает только подключение.
            bool isInternetEnabled = Application.internetReachability != NetworkReachability.NotReachable;

            Debug.Log($"Try to reload Ad: isInternetEnabled = {isInternetEnabled}.");

            if (isInternetEnabled) ReCreateRewardedAd();
        }

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
            $"And now rewardedAd.IsLoaded() is {rewardedAdLoader.IsAdWasLoaded()}");

        commandQueueHandler.SetCommandToQueue(() => OnAdFailedToLoad?.Invoke());


        void TryToReLoadAd()
        {
            if (TryToReLoadAdRoutine == null) TryToReLoadAdRoutine = StartCoroutine(TryToReLoadAdEnumerator());
        };

        commandQueueHandler.SetCommandToQueue(TryToReLoadAd);
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

        commandQueueHandler.SetCommandToQueue(() => ReCreateRewardedAd());
    }

    private void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        Debug.Log($"HandleRewardedAdClosed event received. MustRewardPlayer = {mustRewardPlayer}");

        commandQueueHandler.SetCommandToQueue(() => OnAdClosed?.Invoke(mustRewardPlayer));

        // Обнуляю значение награды. Т.к другой поток, помещаю команду в очередь.
        commandQueueHandler.SetCommandToQueue(() => mustRewardPlayer = false);

        commandQueueHandler.SetCommandToQueue(() => ReCreateRewardedAd());

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