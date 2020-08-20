using UnityEngine;

public delegate void StarAmountChange();
public class StarCollector : MonoBehaviour
{
    public event StarAmountChange OnStarAmountChange;

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


    private void Start()
    {
        GameMenu.Instance.GameOverScreen.GameOverStatusScreen.GameOverMenu.OnSavePlayerStats += SaveStarsStats;
    }


    private void OnDestroy()
    {
        GameMenu.Instance.GameOverScreen.GameOverStatusScreen.GameOverMenu.OnSavePlayerStats -= SaveStarsStats;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Star star))
        {
            Stars++;
            star.gameObject.SetActive(false);
        }
    }


    private void SaveStarsStats()
    {
        PlayerDataStorageSafe.Instance.PlayerDataModel.PlayerStats.SaveMaxStarsData(Stars);
        PlayerDataStorageSafe.Instance.PlayerDataModel.PlayerInGamePurchases.SaveTotalStarsData(Stars);
    }
}
