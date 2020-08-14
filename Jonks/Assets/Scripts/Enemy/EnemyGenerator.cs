using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private GameObject pursuer = null;
    private readonly float delay = 20f;
    private Coroutine lifeCycleRoutine = null;

    // Start is called before the first frame update
    void Start()
    {
        if (lifeCycleRoutine == null)
        {
            lifeCycleRoutine = StartCoroutine(LifeCycleEnumerator());
        }
    }


    private IEnumerator LifeCycleEnumerator()
    {
        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;
            ObjectPooler.Instance.SpawnFromPool(pursuer, GameManager.Instance.Centre.gameObject.transform.position, Quaternion.identity);
        }

        //lifeCycleRoutine = null;
    }
}
