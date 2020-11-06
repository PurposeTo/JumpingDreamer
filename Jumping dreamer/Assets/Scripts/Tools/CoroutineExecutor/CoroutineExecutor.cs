using System;
using System.Collections;
using UnityEngine;

public class CoroutineExecutor : MonoBehaviour
{
    public ICoroutineInfo CreateCoroutineInfo(IEnumerator enumerator)
    {
        if (enumerator == null) throw new ArgumentNullException("enumerator");

        CoroutineWithData coroutineWithData = new CoroutineWithData(enumerator);

        return coroutineWithData;
    }


    public ICoroutineInfo StartCoroutineExecution(ICoroutineInfo coroutineInfo)
    {
        if (coroutineInfo is null) throw new ArgumentNullException(nameof(coroutineInfo));

        CoroutineWithData coroutineWithData = (CoroutineWithData)coroutineInfo;

        if (!coroutineWithData.IsExecuting)
        {
            coroutineWithData.SetNewCoroutine(StartCoroutine(WrapperEnumerator(coroutineWithData)));
        }
        else coroutineWithData.OnCoroutineAlredyStarted();


        return coroutineWithData;
    }


    public void BreakCoroutine(ICoroutineInfo coroutineInfo)
    {
        if (coroutineInfo is null) throw new ArgumentNullException(nameof(coroutineInfo));

        CoroutineWithData coroutineWithData = (CoroutineWithData)coroutineInfo;

        coroutineWithData.OnStopCoroutineBefore?.Invoke();

        if (coroutineWithData.IsExecuting)
        {
            StopCoroutine(coroutineWithData.Coroutine);

            coroutineWithData.SetNullToCoroutine();
        }
        else coroutineWithData.OnCoroutineIsAlredyStopped?.Invoke();

        coroutineWithData.OnStopCoroutineAfter?.Invoke();
    }


    private IEnumerator WrapperEnumerator(CoroutineWithData coroutineWithData)
    {
        yield return coroutineWithData.Enumerator;
        coroutineWithData.SetNullToCoroutine();
    }


    public class CoroutineWithData : ICoroutineInfo
    {
        public IEnumerator Enumerator { get; }
        public Coroutine Coroutine { get; private set; } = null;
        public bool IsExecuting => Coroutine != null;


        public CoroutineWithData(IEnumerator enumerator)
        {
            Enumerator = enumerator ?? throw new ArgumentNullException(nameof(enumerator));
        }


        public void SetNewCoroutine(Coroutine coroutine)
        {
            Coroutine = coroutine ?? throw new ArgumentNullException(nameof(coroutine));
        }


        public Action OnCoroutineAlredyStarted { get; set; } = null;
        public Action OnStopCoroutineBefore { get; set; } = null;
        public Action OnStopCoroutineAfter { get; set; } = null;
        public Action OnCoroutineIsAlredyStopped { get; set; } = null;


        public void SetNullToCoroutine() => Coroutine = null;
    }
}
