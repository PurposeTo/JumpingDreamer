using System;

[Obsolete]
public class PooledObject : SuperMonoBehaviour, IPooledObject
{
    // Метод для подписания ИЗВНЕ
    public event Action ObjectSpawnInititialized
    {
        add
        {
            OnObjectSpawnInitialize += value;

            if (IsObjectSpawnInitialized) ExecuteCommandsAndClear(ref OnObjectSpawnInitialize);
        }
        remove
        {
            OnObjectSpawnInitialize -= value;
        }
    }

    private Action OnObjectSpawnInitialize;
    private bool IsObjectSpawnInitialized = false;


    public virtual void OnObjectSpawn() { }

    /// <summary>
    /// Необходимо использовать данный метод взамен OnDisable()
    /// </summary>
    protected virtual void OnDisableWrapped() { }

    // Метод для ObjectPooler-а.
    public virtual void OnObjectSpawnSuper()
    {
        IsObjectSpawnInitialized = true;
        OnObjectSpawn();
        EndOnObjectSpawnExecution();
    }


    private void OnDisable()
    {
        OnDisableSuper();
    }


    // Необходимо отдельным классом, который будет контролировать все вызовы, собирать все OnDisableSuper() только если был переопределен OnDisableWrapped() и вызывать в OnDisable().
    private void OnDisableSuper()
    {
        OnDisableWrapped();
        EndOnDisableExecution();
    }


    private void EndOnObjectSpawnExecution()
    {
        IsObjectSpawnInitialized = true;
        ExecuteCommandsAndClear(ref OnObjectSpawnInitialize);
    }


    private void EndOnDisableExecution()
    {
        IsObjectSpawnInitialized = false;
    }
}
