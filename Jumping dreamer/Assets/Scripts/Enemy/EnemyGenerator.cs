using System.Collections;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private GameObject pursuer = null;
    private readonly float delay = 20f;

    private CoroutineExecutor CoroutineExecutor => CoroutineExecutor.Instance;
    private ICoroutineInfo lifeCycleInfo;

    private void Start()
    {
        lifeCycleInfo = CoroutineExecutor.CreateCoroutineInfo(LifeCycleEnumerator());
        CoroutineExecutor.ContiniousCoroutineExecution(lifeCycleInfo);
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
