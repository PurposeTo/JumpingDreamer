using System;
using System.Collections;
using UnityEngine;
using GoogleMobileAds.Api;

public class CollectRewardsScreen : MonoBehaviour
{
    private GameOverStatusScreen gameOverStatusScreen;

    public void Initialize(GameOverStatusScreen gameOverStatusScreen)
    {
        this.gameOverStatusScreen = gameOverStatusScreen;
    }


    public void CollectRewards()
    {
        // Показать рекламу
        AdMobScript.Instance.ShowRewardVideoAd(hasAdBeenShowed =>
        {
            if (hasAdBeenShowed)
            {
                AdMobScript.Instance.OnCloseAdWait(mustRewardPlayer =>
                {
                    // Стоит ли наградить игрока?
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
                });
            }
            else
            {
                Debug.LogWarning("Ad was not showed cause internet connection lost.");
                gameOverStatusScreen.ShowGameOverMenu();
            }
        });
    }


    public void OpenMainMenu()
    {
        SceneLoader.LoadScene(SceneLoader.MainMenuName);
    }
}
