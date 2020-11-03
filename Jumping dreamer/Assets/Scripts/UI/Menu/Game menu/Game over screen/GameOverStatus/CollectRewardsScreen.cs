using UnityEngine;

public class CollectRewardsScreen : MonoBehaviour
{
    private GameOverStatusScreen gameOverStatusScreen;


    public void Initialize(GameOverStatusScreen gameOverStatusScreen)
    {
        this.gameOverStatusScreen = gameOverStatusScreen;
    }


    public void CollectRewards()
    {
        LoadingWindow adLoadingWindow = null;

        // Показать рекламу
        AdMobScript.Instance.ShowRewardVideoAd(isAdWasReallyLoaded =>
        {
            if (isAdWasReallyLoaded) adLoadingWindow = PopUpWindowGenerator.Instance.CreateLoadingWindow().GetComponent<LoadingWindow>();
        },
        hasAdBeenShowed =>
        {
            adLoadingWindow.TurnOff();

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
                Debug.LogWarning("Ad was not showed cause internet connection lost or got AdFailedLoad");
                gameOverStatusScreen.ShowGameOverMenu();
            }
        });
    }


    public void OpenMainMenu()
    {
        SceneLoader.LoadScene(SceneLoader.MainMenuName);
    }
}
