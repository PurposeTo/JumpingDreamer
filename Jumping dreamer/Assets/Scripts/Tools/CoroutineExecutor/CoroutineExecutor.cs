using System;
using System.Collections;
using UnityEngine;

public class CoroutineExecutor : SingletonMonoBehaviour<CoroutineExecutor>
{
    public ICoroutineInfo CreateCoroutineInfo()
    {
        return CreateCoroutineInfo(null);
    }

    public ICoroutineInfo CreateCoroutineInfo(IEnumerator enumerator)
    {
        CoroutineWithData coroutineWithData = new CoroutineWithData(enumerator);

        return coroutineWithData;
    }

    /// <summary>
    /// Запускает корутину в том случае, если она НЕ выполняется в данный момент.
    /// </summary>
    /// <param name="coroutineInfo"></param>
    /// <param name="enumerator">Позволяет запустить другой IEnumerator</param>
    /// <returns></returns>
    public ICoroutineInfo ContiniousCoroutineExecution(ICoroutineInfo coroutineInfo, IEnumerator enumerator)
    {
        if (coroutineInfo is null) throw new ArgumentNullException(nameof(coroutineInfo));
        if (enumerator is null) throw new ArgumentNullException(nameof(enumerator));

        CoroutineWithData coroutineWithData = (CoroutineWithData)coroutineInfo;
        coroutineWithData.SetEnumerator(enumerator);

        return ContiniousCoroutineExecution(coroutineInfo);
    }


    /// <summary>
    /// Запускает корутину в том случае, если она НЕ выполняется в данный момент.
    /// </summary>
    /// <param name="coroutineInfo"></param>
    /// <returns></returns>
    public ICoroutineInfo ContiniousCoroutineExecution(ICoroutineInfo coroutineInfo)
    {
        if (coroutineInfo is null) throw new ArgumentNullException(nameof(coroutineInfo));

        CoroutineWithData coroutineWithData = (CoroutineWithData)coroutineInfo;

        if (!coroutineWithData.IsExecuting)
        {
            StartNewCoroutine(coroutineWithData);
        }
        else coroutineWithData.OnCoroutineAlredyStarted?.Invoke();


        return coroutineWithData;
    }


    /// <summary>
    /// Перед запуском корутины останавливает её, если она выполнялась на данный момент.
    /// </summary>
    /// <param name="coroutineInfo"></param>
    /// <param name="enumerator">Позволяет запустить другой IEnumerator</param>
    /// <returns></returns>
    public ICoroutineInfo ReStartCoroutineExecution(ICoroutineInfo coroutineInfo, IEnumerator enumerator)
    {
        if (enumerator is null) throw new ArgumentNullException(nameof(enumerator));

        CoroutineWithData coroutineWithData = (CoroutineWithData)coroutineInfo;
        coroutineWithData.SetEnumerator(enumerator);

        return ReStartCoroutineExecution(coroutineInfo);
    }


    /// <summary>
    /// Перед запуском корутины останавливает её, если она выполнялась на данный момент.
    /// </summary>
    /// <param name="coroutineInfo"></param>
    /// <returns></returns>
    public ICoroutineInfo ReStartCoroutineExecution(ICoroutineInfo coroutineInfo)
    {
        if (coroutineInfo is null) throw new ArgumentNullException(nameof(coroutineInfo));

        CoroutineWithData coroutineWithData = (CoroutineWithData)coroutineInfo;

        if (coroutineWithData.IsExecuting) BreakCoroutine(coroutineWithData);

        StartNewCoroutine(coroutineWithData);

        return coroutineWithData;
    }


    /// <summary>
    /// Останавливает корутину.
    /// </summary>
    /// <param name="coroutineInfo"></param>
    public void BreakCoroutine(ICoroutineInfo coroutineInfo)
    {
        if (coroutineInfo is null) throw new ArgumentNullException(nameof(coroutineInfo));

        CoroutineWithData coroutineWithData = (CoroutineWithData)coroutineInfo;

        if (coroutineWithData.IsExecuting)
        {
            StopCoroutine(coroutineWithData.Coroutine);

            coroutineWithData.SetNullToCoroutine();
        }
        else coroutineWithData.OnCoroutineIsAlredyStopped?.Invoke();

        coroutineWithData.OnStopCoroutine?.Invoke();
    }


    private void StartNewCoroutine(CoroutineWithData coroutineWithData)
    {
        coroutineWithData.SetNewCoroutine(StartCoroutine(WrapperEnumerator(coroutineWithData)));
    }


    private IEnumerator WrapperEnumerator(CoroutineWithData coroutineWithData)
    {
        yield return coroutineWithData.Enumerator;
        coroutineWithData.SetNullToCoroutine();
    }


    private class CoroutineWithData : ICoroutineInfo
    {
        public IEnumerator Enumerator { get; private set; } = null;
        public Coroutine Coroutine { get; private set; } = null;
        public bool IsExecuting => Coroutine != null;


        public CoroutineWithData() : this(null) { }

        public CoroutineWithData(IEnumerator enumerator)
        {
            Enumerator = enumerator;
        }


        public void SetNewCoroutine(Coroutine coroutine)
        {
            Coroutine = coroutine ?? throw new ArgumentNullException(nameof(coroutine));
        }


        public void SetEnumerator(IEnumerator enumerator)
        {
            Enumerator = enumerator ?? throw new ArgumentNullException(nameof(enumerator));
        }


        /// <summary>
        /// Выполняется во время выполнении метода ContiniousCoroutineExecution, 
        /// в случае, если корутина уже была запущена.
        /// </summary>
        public Action OnCoroutineAlredyStarted { get; set; } = null;
        /// <summary>
        /// Выполняется во время выполнении метода BreakCoroutine,
        /// после остановки корутины.
        /// </summary>
        public Action OnStopCoroutine { get; set; } = null;
        /// <summary>
        /// Выполняется во время выполнении метода BreakCoroutine,
        /// в случае, если корутина УЖЕ была остановлена.
        /// </summary>
        public Action OnCoroutineIsAlredyStopped { get; set; } = null;


        public void SetNullToCoroutine() => Coroutine = null;
    }
}
