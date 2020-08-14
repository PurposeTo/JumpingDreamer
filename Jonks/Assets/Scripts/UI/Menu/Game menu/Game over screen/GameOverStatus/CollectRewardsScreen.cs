using System;
using System.Collections;
using UnityEngine;
using GoogleMobileAds.Api;

public class CollectRewardsScreen : MonoBehaviour
{
    private GameOverStatusScreen gameOverStatusScreen;

    private bool mustRewardPlayer = false; // bool - показывали ли уже рекламу

    private bool isAdClosedByPlayer = false;
    private Coroutine OnCloseAdWaitCoroutine;


    private void Start()
    {
        // Called when the user should be rewarded for watching a video.
        RewardBasedVideoAd.Instance.OnAdRewarded += HandleRewardBasedVideoRewarded;
        // Called when the ad is closed.
        RewardBasedVideoAd.Instance.OnAdClosed += OnCloseAd;
    }


    private void OnDestroy()
    {
        RewardBasedVideoAd.Instance.OnAdRewarded -= HandleRewardBasedVideoRewarded;
        RewardBasedVideoAd.Instance.OnAdClosed -= OnCloseAd;
    }


    public void Initialize(GameOverStatusScreen gameOverStatusScreen)
    {
        this.gameOverStatusScreen = gameOverStatusScreen;
    }


    public void CollectRewards()
    {
        // Показать рекламу

        if (RewardBasedVideoAd.Instance.IsLoaded())
        {
            OnCloseAdWait();
            RewardBasedVideoAd.Instance.Show();
        }
        else
        {
            Debug.LogError("Ad was loaded, but now it isn't");
            gameOverStatusScreen.ShowGameOverMenu();
        }
    }

    public void OpenMainMenu()
    {
        SceneLoader.LoadScene(SceneLoader.MainMenuName);
    }


    private void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        //Если игрок посмотрел рекламу, наградить его
        mustRewardPlayer = true;
    }


    private void OnCloseAd(object sender, EventArgs args)
    {
        // При закрытии рекламы
        isAdClosedByPlayer = true;
    }

    private void OnCloseAdWait()
    {
        if (OnCloseAdWaitCoroutine == null) OnCloseAdWaitCoroutine = StartCoroutine(OnCloseAdWaitEnumerator());
        else Debug.LogError("OnCloseAdWaitCoroutine is already starting!");
    }


    private IEnumerator OnCloseAdWaitEnumerator()
    {
        yield return new WaitUntil(() => isAdClosedByPlayer);
        isAdClosedByPlayer = false;

        if (mustRewardPlayer)
        {
            // Если должны наградить, то показать GameOverMenu
            gameOverStatusScreen.ShowGameOverMenu();
        }
        else
        {
            // Если нет, то показать экран с надписью: "Вы отказались от награды. Желаете возродиться? <Кнопка возродиться> <Кнопка выйти в меню>"
            gameOverStatusScreen.ShowRefuseToViewAdsScreen();
        }

        OnCloseAdWaitCoroutine = null;
    }
}
