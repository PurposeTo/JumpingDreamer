using System;
using System.Linq;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void DisableBlinking();
[RequireComponent(typeof(Animator))]
public class AnimatorBlinkingController : MonoBehaviour
{

    private Animator animator;
    private const string isBlinking = "isBlinking";
    private const string haveEnableState = "haveEnableState";
    private const string haveDisableState = "haveDisableState";
    private const float blinkingAnimationEnterExitDuration = 1f; // Длительность входа или выхрда анимации мерцания - 1 секунда
    private const float blinkingAnimationLoopDuration = 2f; // Длительность анимации мерцания - 2 секунды

    private bool isHasALimitedDuration = false;
    private int amountOfLoopsToExit = 1;
    private int currentLoopCount = 0; // Значение вычисляется в конце петли анимации

    public event DisableBlinking OnDisableBlinking;


    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }


    private void OnEnable()
    {
        CheckEmptyStates();

    }


    public enum DurationType
    {
        Loops,
        Seconds
    }


    public void StartBlinking(bool unscaledTime)
    {
        if (unscaledTime) animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        else animator.updateMode = AnimatorUpdateMode.Normal;

        animator.SetBool(isBlinking, true);
    }


    public void StopBlinking()
    {
        animator.SetBool(isBlinking, false);
    }


    public void OnDisableBlinkingEventInvoke()
    {
        OnDisableBlinking?.Invoke();
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
}
