using System;
using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorBlinkingController : MonoBehaviour
{

    private Animator animator;
    private const string entryState = "Entry state";
    private const string enableBlinkingState = "Enable blinking";
    private const string blinkingState = "Blinking";
    private const string disableBlinkingState = "Disable blinking";
    private const string isBlinking = "isBlinking";
    private const string haveEnableState = "haveEnableState";
    private const string haveDisableState = "haveDisableState";
    private const float blinkingAnimationEnterExitDuration = 1f; // Длительность входа или выхрда анимации мерцания - 1 секунда
    private const float blinkingAnimationLoopDuration = 2f; // Длительность анимации мерцания - 2 секунды

    private bool isHasALimitedDuration = false;
    private int amountOfLoopsToExit = 1;
    private int currentLoopCount = 0; // Значение вычисляется в конце петли анимации

    public event Action OnDisableBlinking;

    private Coroutine stopBlinkingCoroutine;


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


    public void StopBlinking(bool safety)
    {
        if (safety)
        {
            if (stopBlinkingCoroutine == null) stopBlinkingCoroutine = StartCoroutine(StopBlinkingEnumerator());
        }
        else
        {
            animator.SetBool(isBlinking, false);
        }
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


    // Данный метод предусмотрен для animation event. Не менять имя! Аниматор обращается по стринге, имени метода!
    private void CheckLoopAnimator()
    {
        currentLoopCount++;

        if (isHasALimitedDuration)
        {
            if (currentLoopCount >= amountOfLoopsToExit)
            {
                StopBlinking(true);
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

        yield return new WaitWhile(() => stateInfo.IsName(enableBlinkingState));
        animator.SetBool(isBlinking, false);
    }
}
