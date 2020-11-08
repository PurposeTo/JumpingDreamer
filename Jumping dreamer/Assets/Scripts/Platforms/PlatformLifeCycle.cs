using System;
using System.Collections;
using UnityEngine;

public class PlatformLifeCycle : SuperMonoBehaviour, IPooledObject
{
    private protected AnimatorBlinkingController animatorBlinkingController;
    private readonly float blinkingAnimationSeconds = 1.25f;

    private ICoroutineInfo lifeCycleRoutineInfo;
    private Predicate<float> IsAlive;

    private PlatformCauseOfDestroyCreator platformCauseOfDestroyCreator;
    private float lifeTime = 0f;

    private float checkingParameter = 0f; // Проверяться будет либо от времени, либо от высоты... Задать значение. 


    protected override void AwakeSuper()
    {
        lifeCycleRoutineInfo = CreateCoroutineInfo(LifeCycleEnumerator());
        animatorBlinkingController = GetComponent<AnimatorBlinkingController>();
        SetAnimationConfings();
        animatorBlinkingController.OnDisableBlinking += DisableObject;
    }


    void IPooledObject.OnObjectSpawn()
    {
        lifeTime = 0f;
        platformCauseOfDestroyCreator = new PlatformCauseOfDestroyCreator();

        animatorBlinkingController.EnableAlphaColor();

        ContiniousCoroutineExecution(ref lifeCycleRoutineInfo);
    }


    private void OnDestroy()
    {
        animatorBlinkingController.OnDisableBlinking -= DisableObject;
    }


    private void Update()
    {
        lifeTime += Time.deltaTime;
    }


    private IEnumerator LifeCycleEnumerator()
    {
        // Проблемы с инициализацией!
        PlatformConfigsData.PlatformCauseOfDestroy platformCauseOfDestroy = WorldGenerationRulesController.Instance.PlatformGeneratorPresenter.PlatformGeneratorConfigs.PlatformConfigs.PlatformCauseOfDestroy;

        yield return SetCauseOfDestroy(platformCauseOfDestroy);

        yield return new WaitWhile(() => IsAlive(checkingParameter));

        animatorBlinkingController.StartBlinking(false);
    }


    private IEnumerator SetCauseOfDestroy(PlatformConfigsData.PlatformCauseOfDestroy platformCauseOfDestroy)
    {
        switch (platformCauseOfDestroy)
        {
            case PlatformConfigsData.PlatformCauseOfDestroy.AsTimePasses:
            case PlatformConfigsData.PlatformCauseOfDestroy.NoLifeTime:
                checkingParameter = lifeTime;

                IsAlive = platformCauseOfDestroyCreator.GetCauseOfDestroyByTime(platformCauseOfDestroy);

                break;
            case PlatformConfigsData.PlatformCauseOfDestroy.VerticalCauseOfDestroy:
                // Проблемы с инициализацией!
                checkingParameter = GameObjectsHolder.Instance.Centre.GetToCentreMagnitude(transform.position);

                if (gameObject.TryGetComponent(out VerticalMotion verticalMotion))
                {
                    // Эту штуку тожно нужно ожидать
                    yield return new WaitUntil(() => verticalMotion.IsInitialized);

                    // Должно выполняться после VerticalMotion.SetMotionConfigs, тк как будет зависеть от него
                    IsAlive = platformCauseOfDestroyCreator.GetCauseOfDestroyByHight(verticalMotion.GetVerticalCauseOfDestroy());
                }

                break;
            default:
                break;
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
        Debug.Log($"Кря! {gameObject.transform.GetInstanceID()} Disable");
        gameObject.SetActive(false);
    }
}
