using System;
using System.Collections;
using UnityEngine;

/*
 * Если кидает NRE на coroutineExecutor, то скорее всего, к данному классу произошло обращение до выполнения метода AwakeSuper()
 */
public class SuperMonoBehaviour : MonoBehaviour
{
    public event Action AwakeInititialized
    {
        add
        {
            OnAwakeInititialize += value;

            if (IsAwakeInitialized) ExecuteCommandsAndClear(ref OnAwakeInititialize);
        }
        remove
        {
            OnAwakeInititialize -= value;
        }
    }

    public event Action StartInititialized
    {
        add
        {
            OnStartInititialize += value;

            if (IsStartInitialized) ExecuteCommandsAndClear(ref OnStartInititialize);
        }
        remove
        {
            OnStartInititialize -= value;
        }
    }

    private Action OnAwakeInititialize;
    private bool IsAwakeInitialized = false;

    private Action OnStartInititialize;
    private bool IsStartInitialized = false;

    private CoroutineExecutor coroutineExecutor;


    /// <summary>
    /// Необходимо использовать данный метод взамен Awake()
    /// </summary>
    protected virtual void AwakeWrapped() { }
    /// <summary>
    /// Необходимо использовать данный метод взамен Start()
    /// </summary>
    protected virtual void StartWrapped() { }


    private void Awake()
    {
        AwakeSuper();
    }


    private void Start()
    {
        StartSuper();
    }


    // Необходимо отдельным классом, который будет контролировать все вызовы, собирать все AwakeSuper() только если был переопределен AwakeWrapped() и вызывать в Awake().
    private void AwakeSuper()
    {
        InitializingCreatedObject();
        AwakeWrapped();
        EndAwakeExecution();
    }

    // Необходимо отдельным классом, который будет контролировать все вызовы, собирать все StartSuper() только если был переопределен StartWrapped() и вызывать в Start().
    private void StartSuper()
    {
        StartWrapped();
        EndStartExecution();
    }


    private void InitializingCreatedObject()
    {
        coroutineExecutor = new CoroutineExecutor(this);
    }


    private void EndAwakeExecution()
    {
        IsAwakeInitialized = true;
        ExecuteCommandsAndClear(ref OnAwakeInititialize);
    }


    private void EndStartExecution()
    {
        IsStartInitialized = true;
        ExecuteCommandsAndClear(ref OnStartInititialize);
    }


    protected void ExecuteCommandsAndClear(ref Action action)
    {
        action?.Invoke();
        action = null;
    }

    #region CoroutineExecutor

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

    #endregion

    //TODO: доделать, когда будет инициализатор вызовов
    #region ObjectPooler

    private GameObject SpawnFromPool(GameObject prefabKey, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        //TODO: Что бы этот метод корректно работал, необходимо отдельным классом, который будет контролировать все вызовы, инициализировать ObjectPooler раньше всех...
        return ObjectPooler.Instance.SpawnFromPool(prefabKey, position, rotation, parent);
    }
    #endregion
}
