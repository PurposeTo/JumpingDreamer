using System;
using UnityEngine;

public sealed class WaitForDoneRealtime : WaitForDoneBase
{
    public WaitForDoneRealtime(float timeout, Func<bool> predicate) : base(timeout, predicate) { }

    protected override float DeltaTime => Time.unscaledDeltaTime;
}
