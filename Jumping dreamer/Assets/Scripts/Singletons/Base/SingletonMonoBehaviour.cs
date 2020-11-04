using System;
using UnityEngine;

/// <summary> 
/// To access the heir by a static field "Instance".
/// </summary>
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    [SerializeField] private bool dontDestroyOnLoad = false;

    public static T Instance { get; private set; }

    private static readonly CommandQueue commandQueue = new CommandQueue();


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

            commandQueue.TryToExecuteAllCommands((target, methodInfo) =>
            {
                Debug.Log($"{typeof(T).Name} execute command \"{methodInfo.Name}\" from \"{target}\"");
            });
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
    public static void SetCommandToQueue(params Action[] actions)
    {
        if (Instance == null)
        {
            Array.ForEach(actions, action =>
            {
                Debug.Log($"Сommand \"{action?.Method.Name}\" from \"{action?.Target}\" was added to " +
                    $"{typeof(T).Name} commands queue!");
            });

            commandQueue.SetCommandToQueue(actions);
        }
        else Array.ForEach(actions, action => action?.Invoke());
    }
}

/*Пример с GameManager
 * 
 *  public class GameManager : Singleton<GameManager>
{
	protected override void AwakeSingleton() { }
}
 */


/*Пример использования SetCommandToQueue с кешированием команды для правильного отображения метода в логах
 * 
 * void stopGameTime() => GameManager.Instance.SetGameReady(false); // Кеширую для назначения имени команды
 * GameManager.SetCommandToQueue(stopGameTime)
 * 
 */
