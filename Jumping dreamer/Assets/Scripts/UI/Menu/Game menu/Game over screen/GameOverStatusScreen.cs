﻿using UnityEngine;

public class GameOverStatusScreen : MonoBehaviour
{
    public GameOverMenu GameOverMenu;
    public RebornScreen RebornScreen;
    public CollectRewardsScreen CollectRewardsScreen;
    public RefuseToViewAdsScreen RefuseToViewAdsScreen;

    [HideInInspector] public bool isPlayerMustSeeAd = false;

    private void Awake()
    {
        RebornScreen.Initialize(this);
        CollectRewardsScreen.Initialize(this);
    }


    private void OnEnable()
    {
        // Если реклама загружена и игрок еще не использовал ее
        if (AdMobScript.Instance.IsAdWasReallyLoaded())
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
        else // Если реклама не загрузилась
        {
            // Показать окончательный game over screen
            ShowGameOverMenu();
        }
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
