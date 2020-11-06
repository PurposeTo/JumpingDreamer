using System;
using System.Collections;


public interface ICoroutineInfo
{
    IEnumerator Enumerator { get; }
    bool IsExecuting { get; }

    Action OnCoroutineAlredyStarted { get; set; }
    Action OnStopCoroutineBefore { get; set; }
    Action OnStopCoroutineAfter { get; set; }
    Action OnCoroutineIsAlredyStopped { get; set; }
}
