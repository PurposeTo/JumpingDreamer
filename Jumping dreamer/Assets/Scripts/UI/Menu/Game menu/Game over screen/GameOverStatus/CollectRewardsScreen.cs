using System;
using UnityEngine;

public class CollectRewardsScreen : SuperMonoBehaviour
{
    private GameOverStatusScreen gameOverStatusScreen;

    private LoadingWindow adLoadingWindow = null;


    protected override void UpdateWrapped()
    {
        Debug.LogWarning($"UpdateWrapped call. {adLoadingWindow}");
    }


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
                EnableLoadingWindow();

                Debug.LogWarning($"CollectRewards call. {adLoadingWindow}");

            }
        });
    }


    public void OpenMainMenu()
    {
        SingleSceneLoader.LoadScene(SingleSceneLoader.MainMenuName);
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
        if (adLoadingWindow is null) throw new NullReferenceException("adLoadingWindow");

        // При открытии рекламы окошко ожидания необходимо выключить
        DisableLoadingWindow();
    }


    private void OnAdFailedToShow()
    {
        Debug.LogWarning($"OnAdFailedToShow call {adLoadingWindow}");
        if (adLoadingWindow is null) throw new NullReferenceException("adLoadingWindow");

        // Если произошла ошибка показа рекламы, то окошко ожидания необходимо выключить и показать GameOverMenu
        DisableLoadingWindow();
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

    private void EnableLoadingWindow()
    {
        adLoadingWindow = PopUpWindowGenerator.Instance.CreateLoadingWindow();
        SubscribeAdMobEvents();
    }


    private void DisableLoadingWindow()
    {
        adLoadingWindow.TurnOff();
        UnsubscribeAdMobEvents();
    }

}
