using UnityEngine;
using GoogleMobileAds.Api;
using Assets.Scripts.Player.Data;
using System;

public delegate void SavePlayerStats();
public class GameOverStatusScreen : MonoBehaviour
{
    public GameOverMenu GameOverMenu;
    public RebornScreen RebornScreen;
    public CollectRewardsScreen CollectRewardsScreen;

    [HideInInspector] public bool isPlayerMustSeeAd = false;

    private RewardBasedVideoAd rewardBasedVideoAd;

    public event SavePlayerStats OnSavePlayerStats;


    private void Awake()
    {
        // Get singleton reward based video ad reference.
        rewardBasedVideoAd = RewardBasedVideoAd.Instance;

        GameOverMenu.Initialize(this);
        RebornScreen.Initialize(this);
        CollectRewardsScreen.Initialize(this);
    }


    private void OnEnable()
    {
        // Если реклама загружена и игрок еще не использовал ее
        if (rewardBasedVideoAd.IsLoaded())
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


    private void ShowRebornScreen()
    {
        GameOverMenu.gameObject.SetActive(false);
        RebornScreen.gameObject.SetActive(true);
        CollectRewardsScreen.gameObject.SetActive(false);
    }


    public void ShowGameOverMenu()
    {
        OnSavePlayerStats?.Invoke();

        GameOverMenu.gameObject.SetActive(true);
        RebornScreen.gameObject.SetActive(false);
        CollectRewardsScreen.gameObject.SetActive(false);
    }


    private void ShowCollectRewardsScreen()
    {
        GameOverMenu.gameObject.SetActive(false);
        RebornScreen.gameObject.SetActive(false);
        CollectRewardsScreen.gameObject.SetActive(true);
    }
}
