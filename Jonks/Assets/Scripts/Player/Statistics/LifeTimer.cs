using UnityEngine;

public class LifeTimer : MonoBehaviour
{
    private SafeFloat lifeTime = 0f;


    private void Start()
    {
        PlayerDataSaver.Instance.OnSavePlayerStats += SaveLifeTimeStats;
    }


    private void OnDestroy()
    {
        PlayerDataSaver.Instance.OnSavePlayerStats -= SaveLifeTimeStats;
    }


    private void Update()
    {
        lifeTime += 1f * Time.deltaTime;
    }


    private void SaveLifeTimeStats()
    {
        PlayerDataLocalStorageSafe.Instance.PlayerDataModel.PlayerStats.SaveLifeTimeData((SafeInt)lifeTime);
    }
}
