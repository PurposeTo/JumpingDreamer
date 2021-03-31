using System;
using Desdiene.SuperMonoBehaviourAsset;
using UnityEngine;

public class CollectRewardsScreen : SuperMonoBehaviour
{
    private GameOverStatusScreen gameOverStatusScreen;

    private void OnDestroy()
    {
        UnsubscribeAdMobEvents();
    }


    public void Constructor(GameOverStatusScreen gameOverStatusScreen)
    {
        this.gameOverStatusScreen = gameOverStatusScreen;
    }


    public void CollectRewards()
    {
        // Показать рекламу
        GoogleAdMobController.Instance.ShowRewardVideoAd(isAdWasReallyLoaded =>
        {
            if (isAdWasReallyLoaded)
            {
                // Если реклама была успешно загружена, то включить окошко ожидания показа рекламы
                StartWaitingRewardAd();
            }
        });
    }


    public void OpenMainMenu()
    {
        SingleSceneLoader.Instance.LoadScene(SingleSceneLoader.MainMenuName);
    }


    private void SubscribeAdMobEvents()
    {
        GoogleAdMobController.Instance.OnAdOpening += OnAdOpening;
        GoogleAdMobController.Instance.OnAdFailedToShow += OnAdFailedToShow;
        GoogleAdMobController.Instance.OnAdClosed += OnAdClosed;
    }


    private void UnsubscribeAdMobEvents()
    {
        GoogleAdMobController.Instance.OnAdOpening -= OnAdOpening;
        GoogleAdMobController.Instance.OnAdFailedToShow -= OnAdFailedToShow;
        GoogleAdMobController.Instance.OnAdClosed -= OnAdClosed;
    }


    private void OnAdOpening()
    {
        EndWaitingRewardAd();
    }


    private void OnAdFailedToShow()
    {
        // Если произошла ошибка показа рекламы, то необходимо перестать ждать и показать GameOverMenu
        EndWaitingRewardAd();
        gameOverStatusScreen.ShowGameOverMenu();
    }


    private void OnAdClosed(bool mustRewardPlayer)
    {
        // Стоит ли наградить игрока?
        if (mustRewardPlayer)
        {
            Debug.Log($"OnAdClosed call: mustRewardPlayer = {mustRewardPlayer}. Than ShowGameOverMenu!");
            // Если должны наградить, то показать GameOverMenu
            gameOverStatusScreen.ShowGameOverMenu();
        }
        else
        {
            Debug.Log($"OnAdClosed call: mustRewardPlayer = {mustRewardPlayer}. Than ShowRefuseToViewAdsScreen!");
            // Если нет, то показать экран с <Кнопка возродиться> <Кнопка выйти в меню>"
            gameOverStatusScreen.ShowRefuseToViewAdsScreen();
        }

    }

    private void StartWaitingRewardAd()
    {
        InternetConnectionWaitingDisplayer.Instance.StartWaiting(this);
        SubscribeAdMobEvents();
    }


    private void EndWaitingRewardAd()
    {
        InternetConnectionWaitingDisplayer.Instance.EndWaiting(this);
        UnsubscribeAdMobEvents();
    }
}
