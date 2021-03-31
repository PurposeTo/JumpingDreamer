using System.Collections;
using Desdiene.Coroutine.CoroutineExecutor;
using Desdiene.ObjectPoolerAsset;
using Desdiene.SuperMonoBehaviourAsset;
using Desdiene.UnityEngineExtension;
using UnityEngine;

public class EnemyGenerator : SuperMonoBehaviour
{
    [SerializeField] private GameObject pursuer = null;
    private readonly float delay = 30f;

    private ICoroutineContainer lifeCycleInfo;


    protected override void StartWrapped()
    {
        lifeCycleInfo = CreateCoroutineContainer();
        ExecuteCoroutineContinuously(lifeCycleInfo, LifeCycleEnumerator());
    }


    private IEnumerator LifeCycleEnumerator()
    {
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            pursuer.SpawnFromPool().transform.position = GameObjectsHolder.Instance.Centre.gameObject.transform.position;
        }
    }
}
