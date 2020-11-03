using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Данный класс гарантирует, что Action-ы, помещенные в очередь, будут выполнены из основного потока
/// </summary>
public class CommandQueueHandler : MonoBehaviour
{
    private Queue<Action> commandsQueue = new Queue<Action>();


    private void Update()
    {
        if (commandsQueue.Count != 0) RunAllActions();
    }


    public void SetCommandToQueue(params Action[] actions)
    {
        if (actions.Length == 0 || actions is null) throw new ArgumentNullException(nameof(actions));

        Array.ForEach(actions, action => this.commandsQueue.Enqueue(action));
    }


    private void RunAllActions()
    {
        for (int i = 0; i < commandsQueue.Count; i++) commandsQueue.Dequeue()?.Invoke();
    }
}
