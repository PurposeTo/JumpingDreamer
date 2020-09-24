using System;
using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorBlinkingController : AnimatorControllerWrapper
{
    private const string entryState = "Entry state";
    private const string enableAlphaColorState = "Enable alpha color";
    private const string blinkingState = "Blinking";
    private const string disableAlphaColorState = "Disable alpha color";

    private const string isBlinking = "isBlinking";
    private const string haveEnableState = "haveEnableState";
    private const string haveDisableState = "haveDisableState";
    private const string enableAlphaColor = "Enable";
    private const string disableAlphaColor = "Disable";

    private const float blinkingAnimationEnterExitDuration = 1f; // Длительность входа или выхода анимации мерцания - 1 секунда
    private const float blinkingAnimationLoopDuration = 2f; // Длительность анимации мерцания - 2 секунды

    private bool isHasALimitedDuration = false;
    private int amountOfLoopsToExit = 1;
    private int currentLoopCount = 0; // Значение вычисляется в конце петли анимации

    public event Action OnDisableBlinking;

    private Coroutine stopBlinkingCoroutine;

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
        if (unscaledTime) animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        else animator.updateMode = AnimatorUpdateMode.Normal;


        animator.SetBool(isBlinking, true);
    }


    public void StopBlinking()
    {
        if (stopBlinkingCoroutine == null) stopBlinkingCoroutine = StartCoroutine(StopBlinkingEnumerator());
    }


    public void EnableAlphaColor()
    {
        animator.SetBool(enableAlphaColor, true);
    }


    public void DisableAlphaColor()
    {
        animator.SetBool(enableAlphaColor, true);
    }


    public void OnDisableBlinkingEventInvoke()
    {
        OnDisableBlinking?.Invoke();
    }


    public void SetManualControl(bool manualControlEnableState, bool manualControlDisableState)
    {
        animator.SetBool(enableAlphaColor, !manualControlEnableState);
        animator.SetBool(disableAlphaColor, !manualControlDisableState);
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
                int _amountOfLoopsToExit = Mathf.RoundToInt(durationValue / blinkingAnimationLoopDuration);
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
        animator.speed = blinkingAnimationLoopDuration / secondsForAnimation;
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


    private void CheckEmptyStates()
    {
        bool isEmptyEnable = animator.runtimeAnimatorController.animationClips.Any(animationClip => animationClip.name.ToLower().Contains("empty enable"));
        bool isEmptyDisable = animator.runtimeAnimatorController.animationClips.Any(animationClip => animationClip.name.ToLower().Contains("empty disable"));

        animator.SetBool(haveEnableState, !isEmptyEnable);
        animator.SetBool(haveDisableState, !isEmptyDisable);
    }


    private IEnumerator StopBlinkingEnumerator()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        yield return new WaitWhile(() => stateInfo.IsName(enableAlphaColorState));
        animator.SetBool(isBlinking, false);

        stopBlinkingCoroutine = null;
    }
}
