using UnityEngine;

/// <summary>
/// Данный класс гарантирует, что Action-ы, помещенные в очередь, будут выполнены из основного потока
/// </summary>
public class CommandQueueMainThreadExecutor : MonoBehaviour
{
    private CommandQueue commandQueue = new CommandQueue();


    private void Awake() => commandQueue.TryToExecuteAllCommands();
    private void Start() => commandQueue.TryToExecuteAllCommands();
    private void Update() => commandQueue.TryToExecuteAllCommands();


    public void SetCommandToQueue(params System.Action[] actions)
    {
        commandQueue.SetCommandToQueue(actions);
    }
}
