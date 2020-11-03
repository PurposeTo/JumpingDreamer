using UnityEngine;

public class GameOverStatusScreen : MonoBehaviour
{
    public GameOverMenu GameOverMenu;
    public RebornScreen RebornScreen;
    public CollectRewardsScreen CollectRewardsScreen;
    public RefuseToViewAdsScreen RefuseToViewAdsScreen;

    private bool isPlayerMustSeeAd = false;

    private void Awake()
    {
        RebornScreen.Constructor(this);
        CollectRewardsScreen.Constructor(this);
    }


    private void OnEnable()
    {
        // Если реклама загружена
        if (GoogleAdMobController.Instance.IsAdWasLoaded())
        {
            // Если игрок еще не использовал возрождение
            if (!isPlayerMustSeeAd)
            {
                // Показать экран с предложением возродиться
                ShowRebornScreen();
            }
            else
            {
                // Показать рекламу при сборе наград
                ShowCollectRewardsScreen();
            }
        }
        else
        {
            // Показать окончательный game over
            ShowGameOverMenu();
        }
    }


    public void SetPlayerMustSeeAdTrue()
    {
        isPlayerMustSeeAd = true;
    }


    public void ShowGameOverMenu()
    {
        GameOverMenu.gameObject.SetActive(true);
        RebornScreen.gameObject.SetActive(false);
        CollectRewardsScreen.gameObject.SetActive(false);
        RefuseToViewAdsScreen.gameObject.SetActive(false);
    }


    public void ShowRefuseToViewAdsScreen()
    {
        GameOverMenu.gameObject.SetActive(false);
        RebornScreen.gameObject.SetActive(false);
        CollectRewardsScreen.gameObject.SetActive(false);
        RefuseToViewAdsScreen.gameObject.SetActive(true);
    }


    private void ShowRebornScreen()
    {
        GameOverMenu.gameObject.SetActive(false);
        RebornScreen.gameObject.SetActive(true);
        CollectRewardsScreen.gameObject.SetActive(false);
        RefuseToViewAdsScreen.gameObject.SetActive(false);
    }


    private void ShowCollectRewardsScreen()
    {
        GameOverMenu.gameObject.SetActive(false);
        RebornScreen.gameObject.SetActive(false);
        CollectRewardsScreen.gameObject.SetActive(true);
        RefuseToViewAdsScreen.gameObject.SetActive(false);
    }
}
