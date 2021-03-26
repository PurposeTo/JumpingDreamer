using System.Collections;
using Desdiene.Coroutine.CoroutineExecutor;
using Desdiene.Object_pooler;
using Desdiene.Super_monoBehaviour;
using UnityEngine;

public class EnemyGenerator : SuperMonoBehaviour
{
    [SerializeField] private GameObject pursuer = null;
    private readonly float delay = 30f;

    private ICoroutineContainer lifeCycleInfo;


    protected override void StartWrapped()
    {
        lifeCycleInfo = CreateCoroutineContainer();
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
