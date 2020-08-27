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
        PlayerDataSaver.Instance.OnSavePlayerStats += SaveStarsStats;
    }


    private void OnDestroy()
    {
        PlayerDataSaver.Instance.OnSavePlayerStats -= SaveStarsStats;
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
        PlayerDataLocalStorageSafe.Instance.PlayerDataModel.PlayerStats.SaveMaxStarsData(Stars);
        PlayerDataLocalStorageSafe.Instance.PlayerDataModel.PlayerInGamePurchases.SaveTotalStarsData(Stars);
    }
}
