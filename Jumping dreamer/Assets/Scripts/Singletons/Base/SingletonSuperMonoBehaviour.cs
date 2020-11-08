using System;
using UnityEngine;

/// <summary> 
/// To access the heir by a static field "Instance".
/// </summary>
public abstract class SingletonSuperMonoBehaviour<T> : SuperMonoBehaviour where T : SingletonSuperMonoBehaviour<T>
{
    [SerializeField] private bool dontDestroyOnLoad = false;

    public static T Instance { get; private set; }


    /// <summary>
    /// Данное событие выполнится тогда, когда Instance будет инициализирован.
    /// Команды выполняются сразу в Awake() после метода AwakeSingleton(), если синглтон не был инициализирован.
    /// </summary>
    public static event Action<T> InitializedInstance
    {
        add
        {
            OnInstanceInitialize += value;

            if (Instance != null) Instance.ExecuteCommandsAndClear();
        }
        remove
        {
            OnInstanceInitialize -= value;
        }
    }

    private static Action<T> OnInstanceInitialize;


    protected override void AwakeSuper()
    {
        if (Instance == null)
        {
            Debug.Log($"Initialize SingletonSuperMonoBehaviour {this}");
            Instance = this as T;
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
            AwakeSingleton();

            ExecuteCommandsAndClear();
        }
        else
        {
            Destroy(gameObject); //Destroy(gameObject.GetComponent<T>());
        }
    }
    protected virtual void AwakeSingleton() { }


    private void ExecuteCommandsAndClear()
    {
        OnInstanceInitialize?.Invoke(Instance);
        OnInstanceInitialize = null;
    }
}

/*Пример с GameManager
 * 
 *  public class GameManager : Singleton<GameManager>
{
	protected override void AwakeSingleton() { }
}
 */
