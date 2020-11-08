using System;
using UnityEngine;

public sealed class WaitForDone : WaitForDoneBase
{
    public WaitForDone(float timeout, Func<bool> predicate) : base(timeout, predicate) { }

    protected override float DeltaTime => Time.deltaTime;
}
