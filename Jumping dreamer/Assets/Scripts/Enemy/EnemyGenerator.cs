using System.Collections;
using UnityEngine;

public class EnemyGenerator : SuperMonoBehaviour
{
    [SerializeField] private GameObject pursuer = null;
    private readonly float delay = 30f;

    private ICoroutineInfo lifeCycleInfo;


    private void Start()
    {
        lifeCycleInfo = CreateCoroutineInfo(LifeCycleEnumerator());
        ContiniousCoroutineExecution(ref lifeCycleInfo);
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
