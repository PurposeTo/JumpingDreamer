using System;
using UnityEngine;

public sealed class WaitForDoneScaledTime : WaitForDone
{
    public WaitForDoneScaledTime(float timeout, Func<bool> predicate) : base(timeout, predicate) { }

    protected override float DeltaTime => Time.deltaTime;
}
