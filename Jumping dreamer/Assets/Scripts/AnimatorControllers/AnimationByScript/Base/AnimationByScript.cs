using UnityEngine;

public abstract class AnimationByScript
{
    private protected readonly SuperMonoBehaviour superMonoBehaviour;

    protected AnimationByScript(SuperMonoBehaviour superMonoBehaviour)
    {
        this.superMonoBehaviour = superMonoBehaviour != null ? superMonoBehaviour : throw new System.ArgumentNullException(nameof(superMonoBehaviour));
    }


    private protected bool unscaledTime = false;
    private protected float deltaTime;

    private protected virtual float AnimationDuration { get; private set; } = 2f; // Какая длительность у анимации? По умолчанию 1 сек?..


    public abstract void StartAnimation();


    public virtual void SetDefaultAnimationConfigs()
    {
        SetTimeScaled(unscaledTime);
        SetAnimationCurve();
    }

    public void SetTimeScaled(bool unscaledTime)
    {
        this.unscaledTime = unscaledTime;

        deltaTime = unscaledTime
            ? Time.unscaledDeltaTime
            : Time.deltaTime;
    }


    private protected abstract void SetAnimationCurve();
}
