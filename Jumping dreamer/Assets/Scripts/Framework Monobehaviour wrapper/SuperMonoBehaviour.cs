﻿using System;
using System.Collections;
using UnityEngine;

/*
 * Если кидает NRE на coroutineExecutor, то скорее всего, к данному классу произошло обращение до выполнения метода AwakeSuper()
 */
public class SuperMonoBehaviour : MonoBehaviour
{
    #region SuperMonoBehaviour tools

    private CoroutineExecutor coroutineExecutor;

    private void InitializingSuperMonoBehaviour()
    {
        coroutineExecutor = new CoroutineExecutor(this);
    }

    #endregion


    #region Awake realization

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

    private Action OnAwakeInititialize;
    private bool IsAwakeInitialized = false;


    /// <summary>
    /// Необходимо использовать данный метод взамен Awake()
    /// </summary>
    protected virtual void AwakeWrapped() { }

    private void EndAwakeExecution()
    {
        IsAwakeInitialized = true;
        ExecuteCommandsAndClear(ref OnAwakeInititialize);
    }

    private void AwakeSuper()
    {
        InitializingSuperMonoBehaviour();
        AwakeWrapped();
        EndAwakeExecution();
    }

    private void Awake()
    {
        AwakeSuper();
    }

    #endregion


    #region OnEnable realization

    public event Action OnEnabling;

    /// <summary>
    /// Необходимо использовать данный метод взамен OnEnable()
    /// </summary>
    protected virtual void OnEnableWrapped() { }

    private void EndOnEnableExecution()
    {
        UpdateManager.AddUpdate(UpdateSuper);
        UpdateManager.AddFixedUpdate(FixedUpdateSuper);
        UpdateManager.AddLateUpdate(LateUpdateSuper);
        OnEnabling?.Invoke();
    }

    private void OnEnableSuper()
    {
        OnEnableWrapped();
        EndOnEnableExecution();
    }

    private void OnEnable()
    {
        OnEnableSuper();
    }

    #endregion


    #region Start realization

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

    private Action OnStartInititialize;
    private bool IsStartInitialized = false;


    /// <summary>
    /// Необходимо использовать данный метод взамен Start()
    /// </summary>
    protected virtual void StartWrapped() { }

    private void EndStartExecution()
    {
        IsStartInitialized = true;
        ExecuteCommandsAndClear(ref OnStartInititialize);
    }

    private void StartSuper()
    {
        StartWrapped();
        EndStartExecution();
    }

    private void Start()
    {
        StartSuper();
    }

    #endregion


    #region OnDisable realization

    public event Action OnDisabling;

    /// <summary>
    /// Необходимо использовать данный метод взамен OnEnable()
    /// </summary>
    protected virtual void OnDisableWrapped() { }

    private void EndOnDisableExecution()
    {
        UpdateManager.RemoveUpdate(UpdateSuper);
        UpdateManager.RemoveFixedUpdate(FixedUpdateSuper);
        UpdateManager.RemoveLateUpdate(LateUpdateSuper);
        OnDisabling?.Invoke();
    }

    private void OnDisableSuper()
    {
        OnDisableWrapped();
        EndOnDisableExecution();
    }

    private void OnDisable()
    {
        OnDisableSuper();
    }

    #endregion


    #region OnDestroy realization

    public event Action OnDestroying;

    /// <summary>
    /// Необходимо использовать данный метод взамен OnDestroy()
    /// </summary>
    protected virtual void OnDestroyWrapped() { }

    private void OnDestroySuper()
    {
        OnDestroyWrapped();
        EndOnDestroyExecution();
    }


    private void EndOnDestroyExecution()
    {
        OnDestroying?.Invoke();
    }


    private void OnDestroy()
    {
        OnDestroySuper();
    }

    #endregion


    #region Update realization

    /// <summary>
    /// Необходимо использовать данный метод взамен Update()
    /// </summary>
    protected virtual void UpdateWrapped() { }

    private void UpdateSuper()
    {
        UpdateWrapped();
    }

    #endregion


    #region FixedUpdate realization

    /// <summary>
    /// Необходимо использовать данный метод взамен FixedUpdate()
    /// </summary>
    protected virtual void FixedUpdateWrapped() { }

    private void FixedUpdateSuper()
    {
        FixedUpdateWrapped();
    }

    #endregion


    #region LateUpdate realization

    /// <summary>
    /// Необходимо использовать данный метод взамен LateUpdate()
    /// </summary>
    protected virtual void LateUpdateWrapped() { }

    private void LateUpdateSuper()
    {
        LateUpdateWrapped();
    }

    #endregion


    protected void ExecuteCommandsAndClear(ref Action action)
    {
        action?.Invoke();
        action = null;
    }

    #region CoroutineExecutor

    /// <summary>
    /// Создаёт "Container" объект для конкретной корутины
    /// </summary>
    /// <returns></returns>
    public ICoroutineContainer CreateCoroutineContainer()
    {
        return coroutineExecutor.CreateCoroutineContainer();
    }


    /// <summary>
    /// Запускает корутину в том случае, если она НЕ выполняется в данный момент.
    /// </summary>
    /// <param name="enumerator">Позволяет запустить другой IEnumerator</param>
    /// <returns></returns>
    public void ExecuteCoroutineContinuously(ref ICoroutineContainer coroutineInfo, IEnumerator enumerator)
    {
        coroutineExecutor.ExecuteCoroutineContinuously(ref coroutineInfo, enumerator);
    }


    /// <summary>
    /// Перед запуском корутины останавливает её, если она выполнялась на данный момент.
    /// </summary>
    /// <param name="enumerator">Позволяет запустить другой IEnumerator</param>
    /// <returns></returns>
    public void ReStartCoroutineExecution(ref ICoroutineContainer coroutineInfo, IEnumerator enumerator)
    {
        coroutineExecutor.ReStartCoroutineExecution(ref coroutineInfo, enumerator);
    }


    /// <summary>
    /// Останавливает корутину.
    /// </summary>
    /// <param name="coroutineInfo"></param>
    public void BreakCoroutine(ref ICoroutineContainer coroutineInfo)
    {
        coroutineExecutor.BreakCoroutine(ref coroutineInfo);
    }


    /// <summary>
    /// Останавливает все корутины на объекте
    /// </summary>
    public void BreakAllCoroutines()
    {
        coroutineExecutor.BreakAllCoroutines();
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
