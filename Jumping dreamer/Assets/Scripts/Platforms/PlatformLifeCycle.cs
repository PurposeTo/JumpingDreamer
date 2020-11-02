using System;
using System.Collections;
using UnityEngine;

public class PlatformLifeCycle : MonoBehaviour, IPooledObject
{
    private protected AnimatorBlinkingController animatorBlinkingController;
    private readonly float blinkingAnimationSeconds = 1.25f;

    private float maxAvailableHight;

    private float lifeTimeToDestroy;
    private readonly float minlifeTime = 10f;
    private readonly float maxlifeTime = 30f;

    private Coroutine lifeCycleRoutine;

    private Func<bool> IsAlive;
    private float lifeTime = 0f;


    private protected virtual void Awake()
    {
        animatorBlinkingController = GetComponent<AnimatorBlinkingController>();
        SetAnimationConfings();
        animatorBlinkingController.OnDisableBlinking += DisableObject;
    }


    private void OnDisable()
    {
        RepairValues();
    }


    private void OnDestroy()
    {
        animatorBlinkingController.OnDisableBlinking -= DisableObject;
    }


    private void Update()
    {
        lifeTime += Time.deltaTime;
    }


    public void SetCauseOfDestroy()
    {
        PlatformConfigsData.PlatformCauseOfDestroy platformCauseOfDestroy = WorldGenerationRulesController.Instance.PlatformGeneratorPresenter.PlatformGeneratorConfigs.PlatformConfigs.PlatformCauseOfDestroy;

        switch (platformCauseOfDestroy)
        {
            case PlatformConfigsData.PlatformCauseOfDestroy.AsTimePasses:
                lifeTimeToDestroy = UnityEngine.Random.Range(minlifeTime, maxlifeTime);
                IsAlive = () => !(lifeTime >= lifeTimeToDestroy);
                break;
            case PlatformConfigsData.PlatformCauseOfDestroy.NoLifeTime:
                lifeTimeToDestroy = minlifeTime / 2.5f;
                IsAlive = () => !(lifeTime >= lifeTimeToDestroy);
                break;
            case PlatformConfigsData.PlatformCauseOfDestroy.VerticalCauseOfDeathControl:
                var varticalCauseOfDead = GetCauseOfDeath(gameObject.GetComponent<VerticalMotion>().Direction);

                switch (varticalCauseOfDead)
                {
                    case PlatformConfigsData.VerticalCauseOfDeathControl.TopBorder:
                        maxAvailableHight = UnityEngine.Random.Range(PlatformGeneratorData.PlatformAvailableHighestArea * (2f / 3f), PlatformGeneratorData.PlatformAvailableHighestArea);
                        IsAlive = () =>
                        !(GameObjectsHolder.Instance.Centre.GetToCentreMagnitude(transform.position) >= maxAvailableHight);
                        break;
                    case PlatformConfigsData.VerticalCauseOfDeathControl.BottomBorder:
                        float minAvailableHight = Centre.CentreRadius * 2f;
                        IsAlive = () =>
                        !(GameObjectsHolder.Instance.Centre.GetToCentreMagnitude(transform.position) <= minAvailableHight);
                        break;
                    default:
                        throw new System.Exception($"{varticalCauseOfDead} is unknown VerticalCauseOfDeathControl!");
                }
                break;
            default:
                break;
        }
    }


    void IPooledObject.OnObjectSpawn()
    {
        animatorBlinkingController.EnableAlphaColor();
        if (lifeCycleRoutine == null) StartCoroutine(LifeCycleEnumerator());
    }


    private void RepairValues()
    {
        lifeTime = 0f;
        maxAvailableHight = PlatformGeneratorData.PlatformAvailableHighestArea;
        IsAlive = null;
    }


    private IEnumerator LifeCycleEnumerator()
    {
        if (gameObject.TryGetComponent(out VerticalMotion verticalMotion)) yield return new WaitUntil(() => verticalMotion.Direction != 0);

        // Должно выполняться после SetMotionConfigs, тк как может зависеть от него
        SetCauseOfDestroy();
        yield return new WaitWhile(() => IsAlive());
        animatorBlinkingController.StartBlinking(false);

        lifeCycleRoutine = null;
    }


    public PlatformConfigsData.VerticalCauseOfDeathControl GetCauseOfDeath(int direction)
    {
        switch (direction)
        {
            case 1:
                return PlatformConfigsData.VerticalCauseOfDeathControl.TopBorder;
            case -1:
                return PlatformConfigsData.VerticalCauseOfDeathControl.BottomBorder;
            default:
                throw new System.Exception($"{direction} is unknown direction!");
        }
    }


    private void SetAnimationConfings()
    {
        animatorBlinkingController.SetBlinkingAnimationSpeed(blinkingAnimationSeconds);
        animatorBlinkingController.SetAnimationDuration(AnimatorBlinkingController.DurationType.Loops, 3);
        animatorBlinkingController.SetManualControl(manualControlEnableState: true, manualControlDisableState: false);
    }


    private void DisableObject()
    {
        gameObject.SetActive(false);
    }
}
