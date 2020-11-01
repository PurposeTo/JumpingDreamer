using System;
using System.Collections;
using UnityEngine;
using GoogleMobileAds.Api;


public class AdShow : MonoBehaviour
{
    public bool IsAdAlreadyShowing => OnCloseAdWaitCoroutine != null;

    private bool mustRewardPlayer = false; // bool - показывали ли уже рекламу
    private bool isAdClosedByPlayer = false;
    private Coroutine OnCloseAdWaitCoroutine;


    private void Start()
    {
        // Called when the user should be rewarded for watching a video.
        RewardBasedVideoAd.Instance.OnAdRewarded += OnAdRewarded;
        // Called when the ad is closed.
        RewardBasedVideoAd.Instance.OnAdClosed += OnCloseAd;
    }


    private void OnDestroy()
    {
        RewardBasedVideoAd.Instance.OnAdRewarded -= OnAdRewarded;
        RewardBasedVideoAd.Instance.OnAdClosed -= OnCloseAd;
    }


    /// <summary>
    /// Подождать закрытие рекламы
    /// </summary>
    /// <param name="mustRewardPlayerCallback"></param>
    public void OnCloseAdWait(Action<bool> mustRewardPlayerCallback)
    {
        if (OnCloseAdWaitCoroutine == null) OnCloseAdWaitCoroutine = StartCoroutine(OnCloseAdWaitEnumerator(mustRewardPlayerCallback));
        else
        {
            Debug.LogError("OnCloseAdWaitCoroutine is already starting!");
            HideAdShowingError();
        }
    }


    /// <summary>
    /// Симуляция награждения игрока после просмотра рекламы - для вызова в случае ошибки отображения рекламы
    /// </summary>
    private void HideAdShowingError()
    {
        OnAdRewarded(null, null);
        OnCloseAd(null, null); 
    }


    private void OnAdRewarded(object sender, Reward args)
    {
        //Если игрок посмотрел рекламу, наградить его
        mustRewardPlayer = true;
    }


    private void OnCloseAd(object sender, EventArgs args)
    {
        // При закрытии рекламы
        isAdClosedByPlayer = true;
    }


    private IEnumerator OnCloseAdWaitEnumerator(Action<bool> mustRewardPlayerCallback)
    {
        // Ожидание закрытия рекламы
        yield return new WaitUntil(() => isAdClosedByPlayer);
        isAdClosedByPlayer = false;

        mustRewardPlayerCallback?.Invoke(mustRewardPlayer);

        mustRewardPlayer = false;
        OnCloseAdWaitCoroutine = null;
    }
}
