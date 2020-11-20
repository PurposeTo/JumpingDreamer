using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlatformLifeCycle : SuperMonoBehaviour, IPooledObject
{
    private AnimatorByScript<FadeAnimation> fadeAnimator;
    private AnimatorByScript<BlinkingLoopAnimation> blinkingLoopAnimator;

    private ICoroutineContainer lifeCycleRoutineInfo;
    private Func<bool> IsAliveState;

    private PlatformCauseOfDestroyDeterminator platformCauseOfDestroyCreator;

    // Debug values
    private float lifeTime = 0f;
    private float hight = 0f;


    protected override void AwakeWrapped()
    {
        fadeAnimator = new AnimatorByScript<FadeAnimation>(new FadeAnimation(this, gameObject.GetComponent<SpriteRendererContainer>()), this);
        blinkingLoopAnimator = new AnimatorByScript<BlinkingLoopAnimation>(new BlinkingLoopAnimation(this, gameObject.GetComponent<SpriteRendererContainer>()), this);
        lifeCycleRoutineInfo = CreateCoroutineContainer();
        lifeCycleRoutineInfo.OnCoroutineAlreadyStarted += () => Debug.LogWarning("Уже запущена!");
    }


    void IPooledObject.OnObjectSpawn()
    {
        platformCauseOfDestroyCreator = new PlatformCauseOfDestroyDeterminator();

        // Рестарт потому, что кроме данного скрипта, платформу также может выключить Broakable
        ExecuteCoroutineContinuously(ref lifeCycleRoutineInfo, LifeCycleEnumerator());
    }


    protected override void UpdateWrapped()
    {
        lifeTime += Time.deltaTime;
        hight = GameObjectsHolder.Instance.Centre.GetToCentreMagnitude(transform.position);
    }


    private IEnumerator LifeCycleEnumerator()
    {
        //fadeAnimator.SetAnimationDuration(10f);
        fadeAnimator.Animation.SetFadeState(FadeAnimation.FadeState.fadeIn);
        fadeAnimator.StartAnimation();
        yield return new WaitWhile(() => fadeAnimator.IsExecuting);

        // Проблемы с инициализацией!
        IPlatformCauseOfDestroyConfigs platformCauseOfDestroy = WorldGenerationRulesController.Instance.PlatformGeneratorPresenter.PlatformGeneratorConfigs.PlatformConfigs.CauseOfDestroy;

        yield return SetCauseOfDestroy(platformCauseOfDestroy);

        yield return new WaitWhile(IsAliveState);

        // Todo: внести в blinkingLoopAnimator -> параметры по умолчанию.
        blinkingLoopAnimator.Animation.SetAnimationDuration(1f);
        blinkingLoopAnimator.Animation.SetLowerAlphaValue(0.25f);
        blinkingLoopAnimator.Animation.SetLoopsCount(3);
        blinkingLoopAnimator.StartAnimation();
        yield return new WaitWhile(() => blinkingLoopAnimator.IsExecuting);

        fadeAnimator.Animation.SetFadeState(FadeAnimation.FadeState.fadeOut);
        fadeAnimator.StartAnimation();
        yield return new WaitWhile(() => fadeAnimator.IsExecuting);

        gameObject.SetActive(false);
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
