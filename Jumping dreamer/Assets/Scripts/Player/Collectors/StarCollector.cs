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
        PlayerDataModelController.Instance.OnSavePlayerStats += SaveStarsStats;
    }


    private void OnDestroy()
    {
        PlayerDataModelController.Instance.OnSavePlayerStats -= SaveStarsStats;
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
        PlayerDataModelController.Instance.GetPlayerDataModel().PlayerStats.SaveMaxStarsData(Stars);
        PlayerDataModelController.Instance.GetPlayerDataModel().PlayerInGamePurchases.SaveTotalStarsData(Stars);
    }
}
