using System;
using UnityEngine;
using GoogleMobileAds.Api;
using System.Collections;

[RequireComponent(typeof(AdShow))]
public class AdMobScript : SingletonMonoBehaviour<AdMobScript>
{
    private const string ADS_ID = "ca-app-pub-8365272256827287~9135876659";
    private const string rewardedVideoAd_ID = "ca-app-pub-8365272256827287/5171106131";
    private const string rewardedVideoAdForTest_ID = "ca-app-pub-3940256099942544/5224354917";

    private RewardBasedVideoAd rewardBasedVideoAd;

    private AdShow adShow;

    private readonly InternetConnectionChecker connectionChecker = new InternetConnectionChecker();

    private bool isAdWasLoaded; // Спасибо AdMob api, который не может корректно проверить загрузку рекламы


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
        StartCoroutine(TryToLoadAdEnumerator()); ;
        adShow = gameObject.GetComponent<AdShow>();
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


    /// <summary>
    /// 
    /// </summary>
    /// <param name="hasAdBeenShowed">Делегат вызывается после попытки показа рекламы.</param>
    public void ShowRewardVideoAd(Action<bool> hasAdBeenShowed) => ShowRewardVideoAd(null, hasAdBeenShowed);


    /// <summary>
    /// 
    /// </summary>
    /// <param name="isAdWasReallyLoadedCallback">Делегат вызывается до попытки показа рекламы.</param>
    /// <param name="hasAdBeenShowed">Делегат вызывается после попытки показа рекламы.</param>
    public void ShowRewardVideoAd(Action<bool> isAdWasReallyLoadedCallback, Action<bool> hasAdBeenShowed)
    {
        bool isAdWasReallyLoaded = IsAdWasReallyLoaded();
        isAdWasReallyLoadedCallback?.Invoke(isAdWasReallyLoaded);

        if (isAdWasReallyLoaded)
        {
            StartCoroutine(connectionChecker.PingGoogleCheckerWithTimeoutEnumerator(isInternetAvaliable =>
            {
                if (isInternetAvaliable) rewardBasedVideoAd.Show();
                hasAdBeenShowed?.Invoke(isInternetAvaliable);
            }));
        }
        else hasAdBeenShowed?.Invoke(isAdWasReallyLoaded);
    }


    /// <summary>
    /// Подождать закрытие рекламы
    /// </summary>
    /// <param name="mustRewardPlayerCallback"></param>
    public void OnCloseAdWait(Action<bool> mustRewardPlayerCallback)
    {
        adShow.OnCloseAdWait(mustRewardPlayerCallback);
    }


    public bool IsAdWasReallyLoaded()
    {
        return rewardBasedVideoAd.IsLoaded() && this.isAdWasLoaded;
    }


    /// <summary>
    /// Перезагрузить рекламу
    /// </summary>
    private void RequestRewardBasedVideo()
    {
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the rewarded video ad with the request.
        rewardBasedVideoAd.LoadAd(request, rewardedVideoAdForTest_ID);
    }


    private IEnumerator TryToLoadAdEnumerator()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(30f);

            // Не учитывает реальный доступ к сети. Учитывает только подключение.
            bool isInternetEnabled = Application.internetReachability != NetworkReachability.NotReachable; 
            if (!IsAdWasReallyLoaded() && isInternetEnabled) RequestRewardBasedVideo();
        }
    }


    #region event calls not from the main thread
    private void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        Debug.Log("HandleRewardBasedVideoLoaded event received");
        isAdWasLoaded = true;
    }


    private void HandleRewardBasedVideoFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log($"HandleRewardBasedVideoFailedToLoad event received with message: {args.Message}");

        // Таким образом, после неудачной загрузки мы сразу пытаемся повторно загрузить рекламу и делаем это один раз
        if (IsAdWasReallyLoaded()) RequestRewardBasedVideo(); 
        isAdWasLoaded = false;
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
