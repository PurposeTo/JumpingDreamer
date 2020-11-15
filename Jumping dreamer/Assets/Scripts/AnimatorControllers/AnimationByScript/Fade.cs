using System;
using System.Collections;
using UnityEngine;

/*
 * 1. Данный скрипт должен останавливать анимацию и сбрасывать все настройки при выключении объекта.
 * 2. Если объект выключается до того, как анимация закончилась, необходимо кинуть варнинг, что так делать нельзя
 */
public class Fade : AnimationByScript
{
    private readonly ComponentWithColor componentWithColor;

    public Fade(SuperMonoBehaviour superMonoBehaviour, ComponentWithColor componentWithColor) : base(superMonoBehaviour)
    {
        this.componentWithColor = componentWithColor != null ? componentWithColor : throw new ArgumentNullException(nameof(componentWithColor));

        fadeInfo = superMonoBehaviour.CreateCoroutineInfo(FadeEnumerator());
        SetDefaultAnimationConfigs();
    }

    public override void StartAnimation()
    {
        superMonoBehaviour.ContiniousCoroutineExecution(ref fadeInfo);
    }


    private ICoroutineInfo fadeInfo;
    private AnimationCurve animationCurve;


    private IEnumerator FadeEnumerator()
    {
        float counter = 0f;
        yield return new WaitWhile(() => NeedAnimating(ref counter));
    }


    /// <summary>
    /// Данный метод анимирует И проверяет, необходимо ли анимировать дальше.
    /// </summary>
    /// <param name="counter"></param>
    /// <returns></returns>
    private bool NeedAnimating(ref float counter)
    {
        float alphaChannel = animationCurve.Evaluate(counter);
        componentWithColor.SetChangedAlphaChannelToColor(alphaChannel);

        counter += deltaTime / AnimationDuration;
        return counter < 1f;  // counter от 0 до 1: начало -> конец -> начало кривой   
    }


    private protected override void SetAnimationCurve()
    {
        animationCurve = GetBlinkingAnimationCurve();
    }


    private AnimationCurve GetBlinkingAnimationCurve()
    {
        AnimationCurve curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1))
        {
            preWrapMode = WrapMode.Clamp,
            postWrapMode = WrapMode.Clamp
        };
        return curve;
    }
}
