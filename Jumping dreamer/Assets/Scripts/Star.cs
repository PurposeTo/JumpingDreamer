using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class Star : SuperMonoBehaviour
{
    private protected Animator animator;

    private float lifeTime;
    private readonly float minLifeTime = 10f;
    private readonly float maxLifeTime = 30f;

    private ICoroutineInfo lifeCoroutineInfo;


    protected override void AwakeSuper()
    {
        animator = GetComponent<Animator>();
    }


    private void OnEnable()
    {
        StarGenerator.InitializedInstance += (Instance) => Instance.NumberOfActiveStars++;

        lifeCoroutineInfo = CreateCoroutineInfo(LifeEnumerator());
        ContiniousCoroutineExecution(ref lifeCoroutineInfo);

        lifeTime = Random.Range(minLifeTime, maxLifeTime);
    }


    private void OnDisable()
    {
        StarGenerator.InitializedInstance += (Instance) => Instance.NumberOfActiveStars--;

        BreakCoroutine(ref lifeCoroutineInfo);
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
