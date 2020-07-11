using Assets.Scripts.Player.Data;
using Assets.Scripts.Player.DataModel;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    private int coins = 0;
    public int Coins
    {
        get => coins; 
        
        private set
        {
            coins = value;
            GameMenu.Instance.PlayerUI.UpdateCoinsText(coins);
        }
    }


    private void Start()
    {
        GameMenu.Instance.PlayerUI.UpdateCoinsText(coins);
        GameMenu.Instance.GameOverScreen.gameObject.GetComponentInChildren<GameOverStatusScreen>().OnSavePlayerStats += SaveCoinsStats;
    }


    private void OnDestroy()
    {
        GameMenu.Instance.GameOverScreen.gameObject.GetComponentInChildren<GameOverStatusScreen>().OnSavePlayerStats -= SaveCoinsStats;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Coin coin))
        {
            Coins++;
            coin.gameObject.SetActive(false);
        }
    }


    private void SaveCoinsStats()
    {
        PlayerStatsDataStorageSafe.Instance.SaveCoinsData(Coins);
    }
}
