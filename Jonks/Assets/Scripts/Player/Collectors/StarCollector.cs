using Assets.Scripts.Player.Data;
using UnityEngine;

public class StarCollector : MonoBehaviour
{
    private int stars = 0;
    public int Stars
    {
        get => stars; 
        
        private set
        {
            stars = value;
            GameMenu.Instance.PlayerUI.UpdateStarsText(stars);
        }
    }


    private void Start()
    {
        GameMenu.Instance.PlayerUI.UpdateStarsText(stars);
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
