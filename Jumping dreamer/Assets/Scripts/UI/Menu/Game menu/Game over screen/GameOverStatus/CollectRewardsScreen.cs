using UnityEngine;

public class CollectRewardsScreen : MonoBehaviour
{
    private GameOverStatusScreen gameOverStatusScreen;

    private LoadingWindow adLoadingWindow = null;


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
                adLoadingWindow = PopUpWindowGenerator.Instance.CreateLoadingWindow();

                SubscribeAdMobEvents();
            }
        });
    }


    public void OpenMainMenu()
    {
        SceneLoader.LoadScene(SceneLoader.MainMenuName);
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
        // При открытии рекламы окошко ожидания необходимо выключить
        adLoadingWindow.TurnOff();
    }


    private void OnAdFailedToShow()
    {
        // Если произошла ошибка показа рекламы, то окошко ожидания необходимо выключить и показать GameOverMenu
        adLoadingWindow.TurnOff();
        gameOverStatusScreen.ShowGameOverMenu();
    }


    private void OnAdClosed(bool mustRewardPlayer)
    {
        // Стоит ли наградить игрока?
        if (mustRewardPlayer)
        {
            // Если должны наградить, то показать GameOverMenu
            gameOverStatusScreen.ShowGameOverMenu();
        }
        else
        {
            // Если нет, то показать экран с <Кнопка возродиться> <Кнопка выйти в меню>"
            gameOverStatusScreen.ShowRefuseToViewAdsScreen();
        }

        UnsubscribeAdMobEvents();
    }
}
