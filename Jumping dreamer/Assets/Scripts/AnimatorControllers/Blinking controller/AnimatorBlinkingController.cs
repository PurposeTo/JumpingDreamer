using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class AnimatorBlinkingController : AnimatorControllerWrapper
{
    private readonly AnimatorBlinkingInitializingConfigs animatorBlinkingInitializingConfigs = new AnimatorBlinkingInitializingConfigs();
    private protected override IAnimatorInitializerConfigs AnimatorInitializerConfigs => animatorBlinkingInitializingConfigs;

    private bool isHasALimitedDuration = false;
    private int amountOfLoopsToExit = 1;
    private int currentLoopCount = 0; // Значение вычисляется в конце петли анимации

    public event Action OnDisableBlinking;

    private ICoroutineInfo stopBlinkingInfo;


    protected override void AwakeWrapped()
    {
        base.AwakeWrapped();
        stopBlinkingInfo = CreateCoroutineInfo(StopBlinkingEnumerator());
    }


    public enum DurationType
    {
        Loops,
        Seconds
    }


    private void OnEnable()
    {
        CheckEmptyStates();
    }


    public void StartBlinking(bool unscaledTime)
    {
        if (unscaledTime)
        {
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        }
        else
        {
            animator.updateMode = AnimatorUpdateMode.Normal;
        }

        animator.SetBool(AnimatorBlinkingData.isBlinking, true);

    }


    public void StopBlinking()
    {
        ContiniousCoroutineExecution(ref stopBlinkingInfo);
    }


    public void EnableAlphaColor()
    {
        animator.SetBool(AnimatorBlinkingData.enableAlphaColor, true);
    }


    public void DisableAlphaColor()
    {
        animator.SetBool(AnimatorBlinkingData.disableAlphaColor, true);
    }


    public void SetManualControl(bool manualControlEnableState, bool manualControlDisableState)
    {
        animatorBlinkingInitializingConfigs.SetManualControl(manualControlEnableState, manualControlDisableState);
    }


    public void SetAnimationDuration(DurationType durationType, int durationValue)
    {
        isHasALimitedDuration = true;

        switch (durationType)
        {
            case DurationType.Loops:
                amountOfLoopsToExit = durationValue;
                break;
            case DurationType.Seconds:
                int _amountOfLoopsToExit = Mathf.RoundToInt(durationValue / AnimatorBlinkingData.blinkingAnimationLoopDuration);
                if (_amountOfLoopsToExit == 0) Debug.LogWarning("Внимание! Вы пытаетесь использовать слишком короткую длительность анимации!");
                amountOfLoopsToExit = _amountOfLoopsToExit;
                break;
            default:
                Debug.LogError($"Unknown durationType {durationType}!");
                break;
        }
    }


    public void SetBlinkingAnimationSpeed(float secondsForAnimation)
    {
        animatorBlinkingInitializingConfigs.SetBlinkingAnimationSpeed(secondsForAnimation);
    }


    // Данный метод предусмотрен для animation event. Не менять имя! Аниматор обращается по стринге, имени метода!
    private void CheckLoopAnimator()
    {
        currentLoopCount++;

        if (isHasALimitedDuration)
        {
            if (currentLoopCount >= amountOfLoopsToExit)
            {
                StopBlinking();
            }
        }
    }


    // Данный метод предусмотрен для animation event. Не менять имя! Аниматор обращается по стринге, имени метода!
    private void OnDisableBlinkingEventInvoke()
    {
        OnDisableBlinking?.Invoke();
    }


    private void CheckEmptyStates()
    {
        bool isEmptyEnable = animator.runtimeAnimatorController.animationClips.Any(animationClip => animationClip.name.ToLower().Contains("empty enable"));
        bool isEmptyDisable = animator.runtimeAnimatorController.animationClips.Any(animationClip => animationClip.name.ToLower().Contains("empty disable"));

        animator.SetBool(AnimatorBlinkingData.haveEnableState, !isEmptyEnable);
        animator.SetBool(AnimatorBlinkingData.haveDisableState, !isEmptyDisable);
    }


    private IEnumerator StopBlinkingEnumerator()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        yield return new WaitWhile(() => stateInfo.IsName(AnimatorBlinkingData.enableAlphaColorState));
        animator.SetBool(AnimatorBlinkingData.isBlinking, false);
    }
}
