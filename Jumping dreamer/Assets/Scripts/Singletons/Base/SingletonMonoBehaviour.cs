using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// To access the heir by a static field "Instance".
/// </summary>
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    [SerializeField] private bool dontDestroyOnLoad = false;

    public static T Instance { get; private set; }

    private static Queue<Action> awakeCommands = new Queue<Action>();


    private void Awake()
    {
        if (Instance == null)
        {
            Debug.Log($"Initialize singletonMonoBehaviour {this}");
            Instance = this as T;
            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
            AwakeSingleton();
            ExecuteCommands();
        }
        else
        {
            Destroy(gameObject); //Destroy(gameObject.GetComponent<T>());
        }
    }
    protected virtual void AwakeSingleton() { }


    /// <summary>
    /// Данный метод гарантирует, что команда, переданная в параметры, будет выполнена синглтоном в независимости от того, был тот инициализирован на момент вызова данного метода или нет.
    /// Команды выполняются сразу после метода AwakeSingleton(), если синглтон не был инициализирован.
    /// </summary>
    public static void SetAwakeCommand(Action action)
    {
        if (action is null) throw new ArgumentNullException(nameof(action));

        if (Instance != null) action?.Invoke();
        else awakeCommands.Enqueue(action);
    }


    private void ExecuteCommands()
    {
        if (awakeCommands != null && awakeCommands.Count != 0)
        {
            for (int i = 0; i < awakeCommands.Count; i++) awakeCommands.Dequeue()?.Invoke();
        }
    }
}

/*Пример с GameManager
 * 
 *  public class GameManager : Singleton<GameManager>
{
	protected override void AwakeSingleton() { }
}
 */
