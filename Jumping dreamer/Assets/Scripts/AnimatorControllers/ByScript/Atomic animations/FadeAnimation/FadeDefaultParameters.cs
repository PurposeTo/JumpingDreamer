using System;

[Obsolete]
public class FadeDefaultParameters : AnimationByScriptDefaultParameters
{
    public FadeAnimator.FadeState FadeState { get; set; } = FadeAnimator.FadeState.fadeIn;
}
