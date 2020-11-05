using System;
using UnityEngine;

public abstract class WaitForDone : CustomYieldInstruction
{
    private readonly Func<bool> predicate;
    private float timeout;

    public WaitForDone(float timeout, Func<bool> predicate)
    {
        this.predicate = predicate;
        this.timeout = timeout;
    }


    protected abstract float DeltaTime { get; }


    private bool WaitForDoneProcess()
    {
        timeout -= DeltaTime;
        return timeout <= 0f || predicate();
    }

    public override bool keepWaiting => !WaitForDoneProcess();
}
/*Пример использования
 * yield return new WaitForDone(timeOut, () => condition);
 * 
 */