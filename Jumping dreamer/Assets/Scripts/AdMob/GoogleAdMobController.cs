using System;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using Debug = UnityEngine.Debug;
using System.Collections;

[RequireComponent(typeof(CommandQueueMainThreadExecutor))]
public class GoogleAdMobController : SingletonSuperMonoBehaviour<GoogleAdMobController>
{
    private IRewardedAdLoader rewardedAdLoader;

    public event Action OnAdOpening;
    public event Action OnAdFailedToShow;
    public event Action OnUserEarnedReward;
    public event Action<bool> OnAdClosed;

    private CommandQueueMainThreadExecutor commandQueueHandler;
    private readonly InternetConnectionChecker connectionChecker = new InternetConnectionChecker();

    private ICoroutineContainer waitForRewardedAdAnsweringInfo;
    private ICoroutineContainer checkInternetConnectionAndShowAdInfo;
    private bool IsLoadAnswer => isLoadOpen || isLoadFailedToShow; // Отвечает ли реклама?
    private bool isLoadOpen = false; // Открылась ли реклама?
    private bool isLoadFailedToShow = false; // Произошла ли ошибка показа рекламы?

    private bool mustRewardPlayer = false;

    protected override void AwakeSingleton()
    {
        waitForRewardedAdAnsweringInfo = CreateCoroutineContainer();
        checkInternetConnectionAndShowAdInfo = CreateCoroutineContainer();

        commandQueueHandler = gameObject.GetComponent<CommandQueueMainThreadExecutor>();
        rewardedAdLoader = new RewardedAdLoader(this, commandQueueHandler);
        InitializeRewardAdActions();
    }


    //private void OnDestroy()
    //{
    //    /* Это синглтон dontDestroyOnLoad, разрушаться (Сам синглтон-объект) он не будет. 
    //     * У объекта-дубликата с новой сцены теряется ссылка на объект RewardedAd и отписка происходить не будет, 
    //     * тк AwakeSingleton в новом объекте, который разрушается, не будет выполнен.
    //     */
    //    //if (rewardedAdLoader.RewardedAd != null) UnsubscribeEvents(rewardedAdLoader.RewardedAd);
    //}


    private void InitializeRewardAdActions()
    {
        rewardedAdLoader.OnAdOpening += OnAdOpening;
        OnAdOpening += () =>
        {
            Debug.Log($"OnAdOpening event received.");
            isLoadOpen = true;
        };

        rewardedAdLoader.OnAdFailedToShow += OnAdFailedToShow;
        OnAdFailedToShow += () =>
        {
            Debug.Log($"OnAdFailedToShow event received.");
            isLoadFailedToShow = true;
        };

        rewardedAdLoader.OnAdClosed += () => OnAdClosed(mustRewardPlayer);
        OnAdClosed += (_) =>
        {
            Debug.Log($"OnAdFailedToShow event received. MustRewardPlayer = {mustRewardPlayer}");
        };

        rewardedAdLoader.OnUserEarnedReward += OnUserEarnedReward;
        OnUserEarnedReward += () =>
        {
            Debug.Log($"OnUserEarnedReward event received.");
            mustRewardPlayer = true; // Игрок был награжден!
        };
    }


    public bool IsAdLoaded()
    {
        return rewardedAdLoader.IsAdLoaded();
    }


    public void ShowRewardVideoAd(Action<bool> isAdWasReallyLoadedCallback)
    {
        mustRewardPlayer = false; // Обнуляю значение награды

        bool isAdWasReallyLoaded = rewardedAdLoader.IsAdLoaded();
        isAdWasReallyLoadedCallback?.Invoke(isAdWasReallyLoaded);

        Debug.Log($"Try to show Ad. isAdLoaded = {isAdWasReallyLoaded}");

        if (isAdWasReallyLoaded)
        {
            ExecuteCoroutineContinuously(ref checkInternetConnectionAndShowAdInfo, CheckInternetConnectionAndShowAd());
        }
    }


    private IEnumerator CheckInternetConnectionAndShowAd()
    {
        int operationTimeOut = 4;

        yield return connectionChecker.PingGoogleEnumerator(isInternetAvaliable =>
        {
            if (isInternetAvaliable)
            {
                ExecuteCoroutineContinuously(ref waitForRewardedAdAnsweringInfo, WaitForRewardedAdAnsweringEnumerator());
                rewardedAdLoader.Show();
            }
            else
            {
                Debug.Log($"Force invoke OnAdFailedToShow because internet is not avaliable.");
                OnAdFailedToShow?.Invoke();
            }
        },
        timeOut: operationTimeOut);
    }


    private IEnumerator WaitForRewardedAdAnsweringEnumerator()
    {
        float timeOut = 8f;

        yield return new WaitForDoneRealtime(timeOut, () => IsLoadAnswer);

        // Если не дождались ответа от рекламы
        if (!IsLoadAnswer)
        {
            Debug.Log($"Force invoke OnAdFailedToShow because RewardedAd is not answering.");
            OnAdFailedToShow?.Invoke();
        }

        isLoadOpen = false;
        isLoadFailedToShow = false;
    }


    #region event calls not from the main thread




    #endregion
}