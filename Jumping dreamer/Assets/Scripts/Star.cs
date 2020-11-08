using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class Star : MonoBehaviour
{
    private protected Animator animator;

    private float lifeTime;
    private readonly float minLifeTime = 10f;
    private readonly float maxLifeTime = 30f;

    private CoroutineExecutor CoroutineExecutor => CoroutineExecutor.Instance;
    private ICoroutineInfo lifeCoroutineInfo;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    private void OnEnable()
    {
        StarGenerator.InitializedInstance += (Instance) => Instance.NumberOfActiveStars++;

        CoroutineExecutor.InitializedInstance += (Instance) =>
        {
            lifeCoroutineInfo = CoroutineExecutor.CreateCoroutineInfo(LifeEnumerator());
            lifeCoroutineInfo = CoroutineExecutor.ContiniousCoroutineExecution(lifeCoroutineInfo);
        };

        lifeTime = Random.Range(minLifeTime, maxLifeTime);
    }


    private void OnDisable()
    {
        StarGenerator.InitializedInstance += (Instance) => Instance.NumberOfActiveStars--;

        CoroutineExecutor.InitializedInstance += (Instance) =>
        {
            CoroutineExecutor.BreakCoroutine(lifeCoroutineInfo);
        };
    }


    private IEnumerator LifeEnumerator()
    {
        yield return new WaitForSeconds(lifeTime);
        animator.SetBool("isBlinding", true);
    }


    public void DisableObject()
    {
        gameObject.SetActive(false);
    }
}
