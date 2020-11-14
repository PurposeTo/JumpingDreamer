using System;
using System.Collections;
using UnityEngine;

/*
 * 1. Данный скрипт должен останавливать анимацию и сбрасывать все настройки при выключении объекта.
 * 2. Если объект выключается до того, как анимация закончилась, необходимо кинуть варнинг, что так делать нельзя
 */
public class BlinkingLoop
{
    private readonly SuperMonoBehaviour superMonoBehaviour;

    public BlinkingLoop(SuperMonoBehaviour superMonoBehaviour, SpriteRenderer spriteToBlinking)
    {
        this.superMonoBehaviour = superMonoBehaviour != null ? superMonoBehaviour : throw new System.ArgumentNullException(nameof(superMonoBehaviour));
        this.spriteToBlinking = spriteToBlinking;

        blinkingLoopInfo = superMonoBehaviour.CreateCoroutineInfo(BlinkingLoopEnumerator());
        SetAnimationConfigs();
    }

    public event Action OnAnimationEnd;

    private ICoroutineInfo blinkingLoopInfo;
    private AnimationCurve animationCurve;


    private SpriteRenderer spriteToBlinking; // Позже заменить, т.к. необходима универсальность как для SpriteRenderer, так и для image/text

    #region Настройки данной анимации
    private bool isHasALimitedDuration = false; // Есть ли ограниченная длительность у анимации
    private float animationDuration = 2f; // Какая длительность у анимации
    private int amountOfLoopsToExit = 1; // Количество повторений анимации

    private float lowerAlphaValue = 0.25f; // Нижнее значение альфа-канала при мигании

    private bool unscaledTime = false;
    private float deltaTime;

    #endregion

    
    public void StartAnimation()
    {
        superMonoBehaviour.ContiniousCoroutineExecution(ref blinkingLoopInfo);
    }


    private void SetAnimationConfigs()
    {
        SetDeltaTime(unscaledTime);
        SetBlinkingAnimationCurve();
    }


    private void SetDeltaTime(bool unscaledTime)
    {
        deltaTime = unscaledTime
            ? Time.unscaledDeltaTime
            : Time.deltaTime;
    }


    private IEnumerator BlinkingLoopEnumerator()
    {
        int loopCounter = 0;

        while (!isHasALimitedDuration || loopCounter < amountOfLoopsToExit)
        {
            float counter = 0f; // Процент выполнения анимации
            yield return new WaitWhile(() => NeedAnimating(ref counter));
            loopCounter++;
        }

        OnAnimationEnd?.Invoke();
    }


    /// <summary>
    /// Данный метод анимирует И проверяет, необходимо ли анимировать дальше.
    /// </summary>
    /// <param name="counter"></param>
    /// <returns></returns>
    private bool NeedAnimating(ref float counter)
    {
        float alphaChannel = animationCurve.Evaluate(counter);
        spriteToBlinking.color = GetColorWithChangedAlphaChannel(spriteToBlinking.color, alphaChannel);

        counter += deltaTime / animationDuration;
        return counter < 1f;  // counter от 0 до 1: начало -> конец -> начало кривой   
    }


    private Color GetColorWithChangedAlphaChannel(Color color, float newAlphaChannel)
    {
        color.a = newAlphaChannel;
        return color;
    }


    private void SetBlinkingAnimationCurve()
    {
        animationCurve = GetBlinkingAnimationCurve(lowerAlphaValue);
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="lowerAlphaValue">Нижнее значение альфа-канала при мигании. От 0 до 1.</param>
    private AnimationCurve GetBlinkingAnimationCurve(float lowerAlphaValue)
    {
        AnimationCurve curve = new AnimationCurve(new Keyframe(0, 1), new Keyframe(0.5f, lowerAlphaValue))
        {
            preWrapMode = WrapMode.PingPong,
            postWrapMode = WrapMode.PingPong
        };
        return curve;
    }
}
