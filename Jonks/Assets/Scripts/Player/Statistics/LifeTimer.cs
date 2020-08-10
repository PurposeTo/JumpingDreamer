using UnityEngine;

public class LifeTimer : MonoBehaviour
{
    private SafeFloat lifeTime = 0f;


    private void Start()
    {
        GameMenu.Instance.GameOverScreen.GameOverStatusScreen.GameOverMenu.OnSavePlayerStats += SaveLifeTimeStats;
    }


    private void OnDestroy()
    {
        GameMenu.Instance.GameOverScreen.GameOverStatusScreen.GameOverMenu.OnSavePlayerStats -= SaveLifeTimeStats;
    }


    private void Update()
    {
        lifeTime += 1f * Time.deltaTime;
    }


    private void SaveLifeTimeStats()
    {
        PlayerStatsDataStorageSafe.Instance.SaveLifeTimeData((SafeInt)lifeTime);
    }
}
