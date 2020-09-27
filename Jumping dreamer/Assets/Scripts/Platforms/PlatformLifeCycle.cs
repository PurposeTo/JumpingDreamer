using System.Collections;
using UnityEngine;

public class PlatformLifeCycle : MonoBehaviour, IPooledObject
{
    private protected AnimatorBlinkingController animatorBlinkingController;
    private readonly float blinkingAnimationSpeed = 1.25f;

    private float lifeDictance; // Максимальное расстояние от центра до точки, где платформы еще могут существовать

    private readonly float minlifeDictance = 50f;
    private readonly float maxlifeDictance = 80f;

    private Coroutine lifeCycleRoutine;


    private protected virtual void Awake()
    {
        animatorBlinkingController = GetComponent<AnimatorBlinkingController>();
        SetAnimationConfings();
        animatorBlinkingController.OnDisableBlinking += DisableObject;
    }


    private void OnDestroy()
    {
        animatorBlinkingController.OnDisableBlinking -= DisableObject;
    }


    void IPooledObject.OnObjectSpawn()
    {
        animatorBlinkingController.EnableAlphaColor();
        lifeDictance = Random.Range(minlifeDictance, maxlifeDictance);
        if (lifeCycleRoutine == null) StartCoroutine(LifeCycleEnumerator());
    }


    private IEnumerator LifeCycleEnumerator()
    {
        yield return new WaitUntil(() => NeedDestroy());
        animatorBlinkingController.StartBlinking(false);

        lifeCycleRoutine = null;
    }


    private bool NeedDestroy()
    {
        return (GameManager.Instance.CentreObject.transform.position - transform.position).magnitude >= lifeDictance;
    }


    private void SetAnimationConfings()
    {
        animatorBlinkingController.SetBlinkingAnimationSpeed(blinkingAnimationSpeed);
        animatorBlinkingController.SetAnimationDuration(AnimatorBlinkingController.DurationType.Loops, 3);
        animatorBlinkingController.SetManualControl(manualControlEnableState: true, manualControlDisableState: false);
    }


    private void DisableObject()
    {
        gameObject.SetActive(false);
    }
}
