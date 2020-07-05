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
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Coin coin))
        {
            Coins++;
            coin.gameObject.SetActive(false);
        }
    }
}
