using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlatformLifeCycle : SuperMonoBehaviour, IPooledObject
{
    private FadeAnimator fadeAnimator;
    private BlinkingLoopAnimator blinkingLoopAnimator;

    private ICoroutineInfo lifeCycleRoutineInfo;
    private Func<bool> IsAliveState;

    private PlatformCauseOfDestroyDeterminator platformCauseOfDestroyCreator;

    // Debug values
    private float lifeTime = 0f;
    private float hight = 0f;


    protected override void AwakeWrapped()
    {
        fadeAnimator = new FadeAnimator(this, gameObject.GetComponent<SpriteRendererContainer>());
        blinkingLoopAnimator = new BlinkingLoopAnimator(this, gameObject.GetComponent<SpriteRendererContainer>());
        lifeCycleRoutineInfo = CreateCoroutineInfo();
    }


    void IPooledObject.OnObjectSpawn()
    {
        platformCauseOfDestroyCreator = new PlatformCauseOfDestroyDeterminator();

        // Рестарт потому, что кроме данного скрипта, платформу также может выключить Broakable
        ReStartCoroutineExecutionDisablingGameObject(ref lifeCycleRoutineInfo, LifeCycleEnumerator());
    }


    protected override void UpdateWrapped()
    {
        lifeTime += Time.deltaTime;
        hight = GameObjectsHolder.Instance.Centre.GetToCentreMagnitude(transform.position);
    }


    private IEnumerator LifeCycleEnumerator()
    {
        //fadeAnimator.SetAnimationDuration(10f);
        fadeAnimator.SetFadeState(FadeAnimator.FadeState.fadeIn);
        fadeAnimator.StartAnimation();
        yield return new WaitWhile(() => fadeAnimator.IsExecuting);

        // Проблемы с инициализацией!
        IPlatformCauseOfDestroyConfigs platformCauseOfDestroy = WorldGenerationRulesController.Instance.PlatformGeneratorPresenter.PlatformGeneratorConfigs.PlatformConfigs.CauseOfDestroy;

        yield return SetCauseOfDestroy(platformCauseOfDestroy);

        yield return new WaitWhile(IsAliveState);

        // Todo: внести в blinkingLoopAnimator -> параметры по умолчанию.
        blinkingLoopAnimator.SetAnimationDuration(1f);
        blinkingLoopAnimator.SetLowerAlphaValue(0.25f);
        blinkingLoopAnimator.SetLoopsCount(3);
        blinkingLoopAnimator.StartAnimation();
        yield return new WaitWhile(() => blinkingLoopAnimator.IsExecuting);

        fadeAnimator.SetFadeState(FadeAnimator.FadeState.fadeOut);
        fadeAnimator.StartAnimation();
        yield return new WaitWhile(() => fadeAnimator.IsExecuting);
    }


    private IEnumerator SetCauseOfDestroy(IPlatformCauseOfDestroyConfigs platformCauseOfDestroy)
    {
        Predicate<float> IsAlive;

        switch (platformCauseOfDestroy.ParentTier.Value)
        {
            case PlatformCausesOfDestroy.ByTime:

                var causeOfDestroyByTime = ((PlatformCauseOfDestroyByTime)platformCauseOfDestroy).Value;

                IsAlive = platformCauseOfDestroyCreator.GetCauseOfDestroyByTime(causeOfDestroyByTime);
                IsAliveState = () => IsAlive(lifeTime);

                break;


            case PlatformCausesOfDestroy.ByHight:

                // Проблемы с инициализацией!
                VerticalMotion verticalMotion = gameObject.GetComponent<VerticalMotion>();
                // Эту штуку нужно ожидать
                yield return new WaitUntil(() => verticalMotion.IsInitialized);

                // Должно выполняться после VerticalMotion.SetMotionConfigs, тк как будет зависеть от него
                IsAlive = platformCauseOfDestroyCreator.GetCauseOfDestroyByHight(verticalMotion.GetPlatformCauseOfDestroyByHight());
                IsAliveState = () => IsAlive(hight);


                break;
            default:
                break;
        }
    }
}
