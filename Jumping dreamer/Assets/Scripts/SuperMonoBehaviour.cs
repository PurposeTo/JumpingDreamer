using System.Collections;
using UnityEngine;

/*
 * Если кидает NRE на coroutineExecutor, то скорее всего, к данному классу произошло обращение до выполнения метода Awake()
 */
public class SuperMonoBehaviour : MonoBehaviour
{
    private CoroutineExecutor coroutineExecutor;

    private void Awake()
    {
        InitializingCreatedObject();
        AwakeSuper();
    }


    private void InitializingCreatedObject()
    {
        coroutineExecutor = new CoroutineExecutor(this);
    }


    protected virtual void AwakeSuper() { }


    /// <summary>
    /// Создаёт "Holder" объект для конкретной корутины
    /// </summary>
    /// <returns></returns>
    public ICoroutineInfo CreateCoroutineInfo()
    {
        return CreateCoroutineInfo(null);
    }


    /// <summary>
    /// Создаёт "Holder" объект для конкретной корутины
    /// </summary>
    /// <returns></returns>
    public ICoroutineInfo CreateCoroutineInfo(IEnumerator enumerator)
    {
        return coroutineExecutor.CreateCoroutineInfo(enumerator);
    }


    /// <summary>
    /// Запускает корутину в том случае, если она НЕ выполняется в данный момент.
    /// </summary>
    /// <param name="enumerator">Позволяет запустить другой IEnumerator</param>
    /// <returns></returns>
    public void ContiniousCoroutineExecution(ref ICoroutineInfo coroutineInfo, IEnumerator enumerator)
    {
        coroutineExecutor.ContiniousCoroutineExecution(ref coroutineInfo, enumerator);
    }


    /// <summary>
    /// Запускает корутину в том случае, если она НЕ выполняется в данный момент.
    /// </summary>
    /// <returns></returns>
    public void ContiniousCoroutineExecution(ref ICoroutineInfo coroutineInfo)
    {
        coroutineExecutor.ContiniousCoroutineExecution(ref coroutineInfo);
    }


    /// <summary>
    /// Перед запуском корутины останавливает её, если она выполнялась на данный момент.
    /// </summary>
    /// <param name="enumerator">Позволяет запустить другой IEnumerator</param>
    /// <returns></returns>
    public void ReStartCoroutineExecution(ref ICoroutineInfo coroutineInfo, IEnumerator enumerator)
    {
        coroutineExecutor.ReStartCoroutineExecution(ref coroutineInfo, enumerator);
    }


    /// <summary>
    /// Перед запуском корутины останавливает её, если она выполнялась на данный момент.
    /// </summary>
    /// <returns></returns>
    public void ReStartCoroutineExecution(ref ICoroutineInfo coroutineInfo)
    {
        coroutineExecutor.ReStartCoroutineExecution(ref coroutineInfo);
    }


    /// <summary>
    /// Останавливает корутину.
    /// </summary>
    /// <param name="coroutineInfo"></param>
    public void BreakCoroutine(ref ICoroutineInfo coroutineInfo)
    {
        coroutineExecutor.BreakCoroutine(ref coroutineInfo);
    }
}
