using System;
using System.Collections;
using Desdiene.SuperMonoBehaviourAsset;
using UnityEngine;


public class BlinkingLoopAnimation : AnimationByScript
{
    private readonly ComponentWithColor componentWithColor;

    public BlinkingLoopAnimation(SuperMonoBehaviour superMonoBehaviour, ComponentWithColor componentWithColor) : base(superMonoBehaviour)
    {
        this.componentWithColor = componentWithColor != null ? componentWithColor : throw new ArgumentNullException(nameof(componentWithColor));
    }

    #region Параметры данной анимации

    private bool isHasALimitedDuration = false; // Есть ли ограниченная длительность у анимации
    private int amountOfLoopsToExit = 1; // Количество повторений анимации
    protected override float AnimationDuration { get; set; } = 2f;
    private float lowerAlphaValue = 0.25f; // Нижнее значение альфа-канала при мигании

    #endregion


    protected override void SetDefaultAnimationCreatingParameters()
    {
        lowerAlphaValue = 0.25f;
    }


    protected override void SetDefaultAnimationExecutionParameters()
    {
        isHasALimitedDuration = false;
        amountOfLoopsToExit = 1;
        AnimationDuration = 2f;
    }


    /// <summary>
    /// Задать количество повторений мигания
    /// </summary>
    public void SetLoopsCount(int loopsCount)
    {
        ChangeAnimationExecutionParameters(() =>
        {
            isHasALimitedDuration = true;
            amountOfLoopsToExit = loopsCount;
        });
    }

    public void SetInfiniteNumberOfLoops()
    {
        ChangeAnimationExecutionParameters(() =>
        {
            isHasALimitedDuration = false;
            amountOfLoopsToExit = 1;
        });
    }


    /// <summary>
    /// Задать нижнее значение альфа-канала. Рекомендуется использовать 0f / 0.25f / 0.5f
    /// </summary>
    /// <param name="lowerAlphaValue"></param>
    public void SetLowerAlphaValue(float lowerAlphaValue)
    {
        ChangeAnimationCreatingParameters(() => this.lowerAlphaValue = lowerAlphaValue);
    }


    public override IEnumerator AnimationEnumerator()
    {
        int loopCounter = 0;

        while (!isHasALimitedDuration || loopCounter < amountOfLoopsToExit)
        {
            float counter = 0f; // Процент выполнения анимации
            yield return new WaitWhile(() => NeedAnimating(ref counter));
            loopCounter++;
        }
    }


    protected override AnimationCurve GetAnimationCurve() => GetBlinkingAnimationCurve(lowerAlphaValue);




    /// <summary>
    /// Данный метод анимирует И проверяет, необходимо ли анимировать дальше.
    /// </summary>
    /// <param name="counter"></param>
    /// <returns></returns>
    private bool NeedAnimating(ref float counter)
    {
        float alphaChannel = AnimationCurve.Evaluate(counter);
        componentWithColor.SetChangedAlphaChannelToColor(alphaChannel);

        counter += GetDeltaTime() / AnimationDuration;

        return counter < 1f;  // counter от 0 до 1: начало -> конец -> начало кривой   
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="lowerAlphaValue">Нижнее значение альфа-канала при мигании. От 0 до 1.</param>
    private AnimationCurve GetBlinkingAnimationCurve(float lowerAlphaValue)
    {
        Keyframe firstFrame = new Keyframe(0, 1); // Объект не прозрачный
        Keyframe secondFrame = new Keyframe(0.5f, lowerAlphaValue); // Объект прозрачный
        Keyframe thirdFrame = new Keyframe(1, 1); // Объект не прозрачный

        AnimationCurve curve = new AnimationCurve(firstFrame, secondFrame, thirdFrame)
        {
            preWrapMode = WrapMode.Clamp,
            postWrapMode = WrapMode.Clamp
        };
        return curve;
    }
}
