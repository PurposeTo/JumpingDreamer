using System;
using UnityEngine;

public sealed class WaitForDoneUnscaledTime : WaitForDone
{
    public WaitForDoneUnscaledTime(float timeout, Func<bool> predicate) : base(timeout, predicate) { }

    protected override float DeltaTime => Time.unscaledDeltaTime;
}
