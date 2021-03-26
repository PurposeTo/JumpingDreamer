using System;
using System.Collections;

namespace Desdiene.Coroutine.CoroutineExecutor
{
    public interface ICoroutineContainer
    {
        IEnumerator Enumerator { get; }
        bool IsExecuting { get; }

        Action OnCoroutineAlreadyStarted { get; set; }
        Action OnStopCoroutine { get; set; }
        Action OnCoroutineIsAlreadyStopped { get; set; }
    }
}
