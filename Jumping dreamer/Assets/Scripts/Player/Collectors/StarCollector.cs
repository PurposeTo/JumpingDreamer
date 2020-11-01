using System;
using UnityEngine;

public class StarCollector : RewardCollector
{
    public event Action OnStarAmountChange;

    private SafeInt stars = 0;
    public SafeInt Stars
    {
        get => stars; 
        
        private set
        {
            stars = value;
            OnStarAmountChange?.Invoke();
        }
    }


    private protected override void Start()
    {
        base.Start();
        PlayerDataModelController.Instance.OnSavePlayerStats += SaveStarsStats;
    }


    private protected override void OnDestroy()
    {
        base.OnDestroy();
        PlayerDataModelController.Instance.OnSavePlayerStats -= SaveStarsStats;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canCollecting && collision.TryGetComponent(out Star star))
        {
            Stars++;
            star.gameObject.SetActive(false);
        }
    }


    private void SaveStarsStats()
    {
        PlayerDataModelController.Instance.GetPlayerDataModel().PlayerStats.SaveMaxStarsData(Stars);
        PlayerDataModelController.Instance.GetPlayerDataModel().PlayerInGamePurchases.SaveTotalStarsData(Stars);
    }
}
