using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorPulsingController : AnimatorControllerWrapper
{
    private const string isPulsing = "isPulsing";


    public void StartPulsing(bool unscaledTime)
    {
        if (unscaledTime) animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        else animator.updateMode = AnimatorUpdateMode.Normal;

        animator.SetBool(isPulsing, true);
    }

}
