using System;
using System.Collections.Generic;
using System.Reflection;

/// <summary>
/// Класс предоставляет интерфейс взаимодействия с очередью команд
/// </summary>
public class CommandQueue
{
    private readonly Queue<Action> commandsQueue = new Queue<Action>();


    public void SetCommandToQueue(params Action[] actions)
    {
        if (actions.Length == 0 || actions is null) throw new ArgumentNullException(nameof(actions));

        Array.ForEach(actions, action => commandsQueue.Enqueue(action));
    }


    public void TryToExecuteAllCommands()
    {
        TryToExecuteAllCommands(null);
    }


    /// <summary>
    /// Перегрузка метода выполнения команд из очереди. 
    /// Позволяет провести дополнительное дествие перед каждым выполнением команды.
    /// Например, вывести лог.
    /// </summary>
    /// <param name="BeforeExecuteCommand">Передает object Target и MethodInfo</param>
    public void TryToExecuteAllCommands(Action<object, MethodInfo> BeforeExecuteCommand)
    {
        if (commandsQueue.Count != 0) ExecuteAllCommands(BeforeExecuteCommand);
    }


    private void ExecuteAllCommands(Action<object, MethodInfo> BeforeExecuteCommand)
    {
        while (commandsQueue.Count > 0)
        {
            Action command = commandsQueue.Dequeue();
            BeforeExecuteCommand?.Invoke(command.Target, command.Method);
            command?.Invoke();
        }
    }
}
