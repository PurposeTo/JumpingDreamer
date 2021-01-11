using UnityEngine;

public class LifeTimer : MonoBehaviour
{
    private SafeFloat lifeTime = 0f;


    private void Start()
    {
        PlayerDataModelController.Instance.SynchronizerNotifier.OnSavePlayerData += SaveLifeTimeStats;
    }


    private void OnDestroy()
    {
        PlayerDataModelController.Instance.SynchronizerNotifier.OnSavePlayerData -= SaveLifeTimeStats;
    }


    private void Update()
    {
        lifeTime += 1f * Time.deltaTime;
    }


    private void SaveLifeTimeStats()
    {
        PlayerDataModelController.Instance.DataInteraction.Setter.Stats.SaveRecordLifeTime((SafeInt)lifeTime);
    }
}
