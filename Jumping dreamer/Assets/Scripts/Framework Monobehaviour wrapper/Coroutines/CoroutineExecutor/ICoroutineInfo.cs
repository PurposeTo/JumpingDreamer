using System;
using System.Collections;


public interface ICoroutineInfo
{
    IEnumerator Enumerator { get; }
    bool IsExecuting { get; }

    Action OnCoroutineAlredyStarted { get; set; }
    Action OnStopCoroutine { get; set; }
    Action OnCoroutineIsAlredyStopped { get; set; }
}
