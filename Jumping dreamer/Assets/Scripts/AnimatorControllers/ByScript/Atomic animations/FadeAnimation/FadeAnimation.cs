﻿using System;
using System.Collections;
using Desdiene.SuperMonoBehaviourAsset;
using UnityEngine;


public class FadeAnimation : AnimationByScript
{
    private readonly ComponentWithColor componentWithColor;

    public FadeAnimation(SuperMonoBehaviour superMonoBehaviour, ComponentWithColor componentWithColor) : base(superMonoBehaviour)
    {
        this.componentWithColor = componentWithColor != null ? componentWithColor : throw new ArgumentNullException(nameof(componentWithColor));
    }

    public enum FadeState
    {
        fadeOut, // "Исчезать". Изменяется альфа-канал от 1 до 0
        fadeIn // "Появляться". Изменяется альфа-канал от 0 до 1
    }

    #region Параметры данной анимации

    private FadeState fadeState;

    #endregion


    protected override void SetDefaultAnimationCreatingParameters()
    {
        ChangeAnimationExecutionParameters(() => fadeState = FadeState.fadeIn);
    }


    public void SetFadeState(FadeState fadeState)
    {
        ChangeAnimationCreatingParameters(() => this.fadeState = fadeState);
    }


    public override IEnumerator AnimationEnumerator()
    {
        float counter = 0f;
        yield return new WaitWhile(() => NeedAnimating(ref counter));
    }


    protected override AnimationCurve GetAnimationCurve() => GetBlinkingAnimationCurve();


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


    private AnimationCurve GetBlinkingAnimationCurve()
    {
        float zeroAlphaChannel = 0f;
        float fullAlphachannel = 1f;
        AnimationCurve curve;

        switch (fadeState)
        {
            case FadeState.fadeOut:
                curve = new AnimationCurve(new Keyframe(0, fullAlphachannel), new Keyframe(1, zeroAlphaChannel));
                break;
            case FadeState.fadeIn:
                curve = new AnimationCurve(new Keyframe(0, zeroAlphaChannel), new Keyframe(1, fullAlphachannel));
                break;
            default:
                curve = null;
                break;
        }

        curve.preWrapMode = WrapMode.Clamp;
        curve.postWrapMode = WrapMode.Clamp;

        return curve;
    }
}
