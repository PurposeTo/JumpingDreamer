using System;
using UnityEngine;

/// <summary>
/// Данный класс гарантирует, что Action-ы, помещенные в очередь, будут выполнены из основного потока
/// </summary>
public class CommandQueueMainThreadExecutor : MonoBehaviour
{
    private readonly CommandQueue commandQueue = new CommandQueue();


    private void Awake() => ExecuteCommands();
    private void Start() => ExecuteCommands();
    private void Update() => ExecuteCommands();


    public void SetCommandToQueue(params Action[] actions)
    {
        commandQueue.SetCommandToQueue("CommandQueueMainThreadExecutor", actions);
    }


    private void ExecuteCommands()
    {
        commandQueue.TryToExecuteAllCommands((target, methodInfo) =>
        {
            Debug.Log($"{GetType()} execute command \"{methodInfo.Name}\" from \"{target}\"");
        });
    }
}
