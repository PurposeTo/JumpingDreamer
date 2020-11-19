using System.Collections;
using UnityEngine;

public class EnemyGenerator : SuperMonoBehaviour
{
    [SerializeField] private GameObject pursuer = null;
    private readonly float delay = 30f;

    private ICoroutineInfo lifeCycleInfo;


    protected override void StartWrapped()
    {
        lifeCycleInfo = CreateCoroutineInfo();
        ExecuteCoroutineContinuously(ref lifeCycleInfo, LifeCycleEnumerator());
    }


    private IEnumerator LifeCycleEnumerator()
    {
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            ObjectPooler.Instance.SpawnFromPool(pursuer, GameObjectsHolder.Instance.Centre.gameObject.transform.position, Quaternion.identity);
        }
    }
}
