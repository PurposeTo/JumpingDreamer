using System;
using System.Collections;
using UnityEngine;

public class PlatformLifeCycle : SuperMonoBehaviour, IPooledObject
{
    private protected AnimatorBlinkingController animatorBlinkingController;
    private readonly float blinkingAnimationSeconds = 1.25f;

    private ICoroutineInfo lifeCycleRoutineInfo;
    private Predicate<float> IsAlive;

    private PlatformCauseOfDestroyDeterminator platformCauseOfDestroyCreator;
    private float lifeTime = 0f;

    private float checkingParameter = 0f; // Проверяться будет либо от времени, либо от высоты... Задать значение. 


    protected override void AwakeWrapped()
    {
        lifeCycleRoutineInfo = CreateCoroutineInfo(LifeCycleEnumerator());
        animatorBlinkingController = GetComponent<AnimatorBlinkingController>();
        SetAnimationConfings();
        animatorBlinkingController.OnDisableBlinking += DisableObject;
    }


    void IPooledObject.OnObjectSpawn()
    {
        lifeTime = 0f;
        platformCauseOfDestroyCreator = new PlatformCauseOfDestroyDeterminator();

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
        IPlatformCauseOfDestroyConfigs platformCauseOfDestroy = WorldGenerationRulesController.Instance.PlatformGeneratorPresenter.PlatformGeneratorConfigs.PlatformConfigs.CauseOfDestroy;

        yield return SetCauseOfDestroy(platformCauseOfDestroy);

        yield return new WaitWhile(() => IsAlive(checkingParameter));

        animatorBlinkingController.StartBlinking(false);
    }


    private IEnumerator SetCauseOfDestroy(IPlatformCauseOfDestroyConfigs platformCauseOfDestroy)
    {
        switch (platformCauseOfDestroy.ParentTier.Value)
        {
            case PlatformCausesOfDestroy.ByTime:
                checkingParameter = lifeTime;

                var causeOfDestroyByTime = ((PlatformCauseOfDestroyByTime)platformCauseOfDestroy).Value;

                IsAlive = platformCauseOfDestroyCreator.GetCauseOfDestroyByTime(causeOfDestroyByTime);

                break;
            case PlatformCausesOfDestroy.ByHight:
                // Проблемы с инициализацией!
                checkingParameter = GameObjectsHolder.Instance.Centre.GetToCentreMagnitude(transform.position);

                if (gameObject.TryGetComponent(out VerticalMotion verticalMotion))
                {
                    // Эту штуку тожно нужно ожидать
                    yield return new WaitUntil(() => verticalMotion.IsInitialized);

                    // Должно выполняться после VerticalMotion.SetMotionConfigs, тк как будет зависеть от него
                    IsAlive = platformCauseOfDestroyCreator.GetCauseOfDestroyByHight(verticalMotion.GetPlatformCauseOfDestroyByHight().Value);
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
        gameObject.SetActive(false);
    }
}
