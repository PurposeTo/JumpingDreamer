using UnityEngine;

public class LifeTimer : MonoBehaviour
{
    private SafeFloat lifeTime = 0f;


    private void Start()
    {
        PlayerDataModelController.Instance.OnSavePlayerStats += SaveLifeTimeStats;
    }


    private void OnDestroy()
    {
        PlayerDataModelController.Instance.OnSavePlayerStats -= SaveLifeTimeStats;
    }


    private void Update()
    {
        lifeTime += 1f * Time.deltaTime;
    }


    private void SaveLifeTimeStats()
    {
        PlayerDataModelController.Instance.GetPlayerDataModel().PlayerStats.SaveLifeTimeData((SafeInt)lifeTime);
    }
}
