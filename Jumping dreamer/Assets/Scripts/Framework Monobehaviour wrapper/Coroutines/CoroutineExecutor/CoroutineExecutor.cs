using System;
using System.Collections;
using UnityEngine;

/*
 * ICoroutineInfo, в методах, запускающих корутину, всегда идут с ref параметром потому, что
 * таким образом будет гарантия того, что ТОЧНО будет переназначена ссылка на объект ICoroutineInfo
 */
public class CoroutineExecutor
{
    private readonly MonoBehaviour monoBehaviour;

    public CoroutineExecutor(MonoBehaviour monoBehaviour)
    {
        this.monoBehaviour = monoBehaviour != null ? monoBehaviour : throw new ArgumentNullException(nameof(monoBehaviour));
    }


    public ICoroutineInfo CreateCoroutineInfo()
    {
        return new CoroutineWithData();
    }


    /// <summary>
    /// Запускает корутину в том случае, если она НЕ выполняется в данный момент.
    /// </summary>
    /// <param name="enumerator">IEnumerator для выполнения</param>
    /// <returns></returns>
    public void ExecuteCoroutineContinuously(ref ICoroutineInfo coroutineInfo, IEnumerator enumerator)
    {
        InitializeContinuouslyExecutingCoroutine(ref coroutineInfo, enumerator, StartNewCoroutine);
    }


    /// <summary>
    /// Запускает корутину в том случае, если она НЕ выполняется в данный момент.
    /// В конце выполнения выключит игровой объект, из которого данная корутина была запущена.
    /// </summary>
    /// <param name="enumerator">IEnumerator для выполнения</param>
    /// <returns></returns>
    public void ExecuteCoroutineContinuouslyDisablingGameObject(ref ICoroutineInfo coroutineInfo, IEnumerator enumerator)
    {
        InitializeContinuouslyExecutingCoroutine(ref coroutineInfo, enumerator, StartNewCoroutineDisablingGameObject);
    }


    /// <summary>
    /// Перед запуском корутины останавливает её, если она выполнялась на данный момент.
    /// </summary>
    /// <param name="enumerator">IEnumerator для выполнения</param>
    /// <returns></returns>
    public void ReStartCoroutineExecution(ref ICoroutineInfo coroutineInfo, IEnumerator enumerator)
    {
        InitializeRestartingCoroutine(ref coroutineInfo, enumerator, StartNewCoroutine);
    }


    /// <summary>
    /// Перед запуском корутины останавливает её, если она выполнялась на данный момент.
    /// В конце выполнения выключит игровой объект, из которого данная корутина была запущена.
    /// </summary>
    /// <param name="enumerator">IEnumerator для выполнения</param>
    /// <returns></returns>
    public void ReStartCoroutineExecutionDisablingGameObject(ref ICoroutineInfo coroutineInfo, IEnumerator enumerator)
    {
        InitializeRestartingCoroutine(ref coroutineInfo, enumerator, StartNewCoroutineDisablingGameObject);
    }


    /// <summary>
    /// Останавливает корутину.
    /// </summary>
    /// <param name="coroutineInfo"></param>
    public void BreakCoroutine(ref ICoroutineInfo coroutineInfo)
    {
        if (coroutineInfo is null) throw new ArgumentNullException(nameof(coroutineInfo));

        CoroutineWithData coroutineWithData = (CoroutineWithData)coroutineInfo;

        if (coroutineWithData.IsExecuting)
        {
            monoBehaviour.StopCoroutine(coroutineWithData.Coroutine);

            coroutineWithData.SetNullToCoroutine();
        }
        else coroutineWithData.OnCoroutineIsAlreadyStopped?.Invoke();

        coroutineWithData.OnStopCoroutine?.Invoke();
    }


    private void InitializeContinuouslyExecutingCoroutine(ref ICoroutineInfo coroutineInfo, IEnumerator enumerator,
        Action<CoroutineWithData> startCoroutine)
    {
        if (coroutineInfo is null) throw new ArgumentNullException(nameof(coroutineInfo));
        if (enumerator is null) throw new ArgumentNullException(nameof(enumerator));

        CoroutineWithData coroutineWithData = (CoroutineWithData)coroutineInfo;
        coroutineWithData.SetEnumerator(enumerator);

        if (!coroutineWithData.IsExecuting)
        {
            startCoroutine?.Invoke(coroutineWithData);
        }
        else coroutineWithData.OnCoroutineAlreadyStarted?.Invoke();
    }


    private void InitializeRestartingCoroutine(ref ICoroutineInfo coroutineInfo, IEnumerator enumerator,
        Action<CoroutineWithData> startCoroutine)
    {
        if (enumerator is null) throw new ArgumentNullException(nameof(enumerator));

        CoroutineWithData coroutineWithData = (CoroutineWithData)coroutineInfo;
        coroutineWithData.SetEnumerator(enumerator);

        if (coroutineInfo.IsExecuting) BreakCoroutine(ref coroutineInfo);

        startCoroutine?.Invoke(coroutineWithData);
    }


    private void StartNewCoroutineDisablingGameObject(CoroutineWithData coroutineWithData)
    {
        coroutineWithData.SetCoroutine(monoBehaviour.StartCoroutine(WrappedEnumeratorDisablingGameObject(coroutineWithData)));
    }


    private void StartNewCoroutine(CoroutineWithData coroutineWithData)
    {
        coroutineWithData.SetCoroutine(monoBehaviour.StartCoroutine(WrappedEnumerator(coroutineWithData)));
    }

    /// <summary>
    /// Выключать объект необходимо после остановки корутины, иначе ObjectPooler может начать использовать объект до того, как корутина фактически будет остановленна.
    /// </summary>
    /// <param name="coroutineWithData"></param>
    /// <returns></returns>
    private IEnumerator WrappedEnumeratorDisablingGameObject(CoroutineWithData coroutineWithData)
    {
        yield return WrappedEnumerator(coroutineWithData);
        monoBehaviour.gameObject.SetActive(false);
    }


    private IEnumerator WrappedEnumerator(CoroutineWithData coroutineWithData)
    {
        yield return coroutineWithData.Enumerator;
        coroutineWithData.SetNullToCoroutine();
    }


    private class CoroutineWithData : ICoroutineInfo
    {
        public IEnumerator Enumerator { get; private set; } = null;
        public Coroutine Coroutine { get; private set; } = null;
        public bool IsExecuting => Coroutine != null;


        public void SetCoroutine(Coroutine coroutine)
        {
            Coroutine = coroutine ?? throw new ArgumentNullException(nameof(coroutine));
        }


        public void SetEnumerator(IEnumerator enumerator)
        {
            Enumerator = enumerator ?? throw new ArgumentNullException(nameof(enumerator));
        }


        /// <summary>
        /// Выполняется во время выполнении метода ExecuteCoroutineContinuously, 
        /// в случае, если корутина уже была запущена.
        /// </summary>
        public Action OnCoroutineAlreadyStarted { get; set; } = null;
        /// <summary>
        /// Выполняется во время выполнении метода BreakCoroutine,
        /// после остановки корутины.
        /// </summary>
        public Action OnStopCoroutine { get; set; } = null;
        /// <summary>
        /// Выполняется во время выполнении метода BreakCoroutine,
        /// в случае, если корутина УЖЕ была остановлена.
        /// </summary>
        public Action OnCoroutineIsAlreadyStopped { get; set; } = null;


        public void SetNullToCoroutine() => Coroutine = null;
    }
}
