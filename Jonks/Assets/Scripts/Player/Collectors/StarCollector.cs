using Assets.Scripts.Player.Data;
using UnityEngine;

public delegate void StarAmountChange();
public class StarCollector : MonoBehaviour
{
    public event StarAmountChange OnStarAmountChange;

    private int stars = 0;
    public int Stars
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
        GameMenu.Instance.GameOverScreen.GameOverStatusScreen.OnSavePlayerStats += SaveStarsStats;
    }


    private void OnDestroy()
    {
        GameMenu.Instance.GameOverScreen.GameOverStatusScreen.OnSavePlayerStats -= SaveStarsStats;
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
        PlayerStatsDataStorageSafe.Instance.SaveStarsData(Stars);
    }
}
