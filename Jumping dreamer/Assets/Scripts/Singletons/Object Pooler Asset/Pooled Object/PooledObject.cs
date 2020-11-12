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

    // Метод для ObjectPooler-а.
    public virtual void OnObjectSpawnSuper()
    {
        IsObjectSpawnInitialized = true;
        OnObjectSpawn();
        EndOnObjectSpawnExecution();
    }


    protected override void OnDisableWrapped()
    {
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
