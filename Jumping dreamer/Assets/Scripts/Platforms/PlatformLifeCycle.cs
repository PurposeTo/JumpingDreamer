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

    private PlatformCauseOfDestroyDeterminator causeOfDestroyDeterminator;

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
        causeOfDestroyDeterminator = new PlatformCauseOfDestroyDeterminator();

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


    public void SetCauseOfDestroy(PlatformCausesOfDestroy platformCauseOfDestroy)
    {
        Predicate<float> IsAlive = causeOfDestroyDeterminator.GetCauseOfDestroy(platformCauseOfDestroy);

        switch (platformCauseOfDestroy)
        {
            case PlatformCausesOfDestroy.AsTimePasses:
            case PlatformCausesOfDestroy.NoLifeTime:
                IsAliveState = () => IsAlive(lifeTime);
                break;
            case PlatformCausesOfDestroy.TopBorder:
            case PlatformCausesOfDestroy.BottomBorder:
                IsAliveState = () => IsAlive(hight);
                break;
            default:
                throw new Exception($"{platformCauseOfDestroy} is unknown causeOfDestroy!");
        }
    }
}
