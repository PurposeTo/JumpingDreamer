using System;
using UnityEngine;

public class AnimatorBlinkingInitializingConfigs : IAnimatorInitializerConfigs
{
    public event Action OnConfigsChanged;

    private Animator animator;
    private float animatorSpeed = 1f;
    private bool manualControlDisableState = false;
    private bool manualControlEnableState = false;


    public void SetAnimator(Animator animator)
    {
        this.animator = animator;
    }


    void IAnimatorInitializerConfigs.InitializeConfigs()
    {
        animator.speed = animatorSpeed;
        animator.SetBool(AnimatorBlinkingData.enableAlphaColor, !manualControlEnableState);
        animator.SetBool(AnimatorBlinkingData.disableAlphaColor, !manualControlDisableState);
    }


    public void SetBlinkingAnimationSpeed(float secondsForAnimation)
    {
        animatorSpeed = AnimatorBlinkingData.blinkingAnimationLoopDuration / secondsForAnimation;
        OnConfigsChanged?.Invoke();
    }


    public void SetManualControl(bool manualControlEnableState, bool manualControlDisableState)
    {
        this.manualControlEnableState = manualControlEnableState;
        this.manualControlDisableState = manualControlDisableState;
        OnConfigsChanged?.Invoke();
    }
}
