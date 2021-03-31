using System;
using Desdiene.Coroutine.WaitForDone.Base;
using UnityEngine;

namespace Desdiene.Coroutine.WaitForDone
{
    public sealed class WaitForDoneRealtime : WaitForDoneBase
    {
        public WaitForDoneRealtime(float timeout, Func<bool> predicate) : base(timeout, predicate) { }

        protected override float DeltaTime => Time.unscaledDeltaTime;
    }
}
