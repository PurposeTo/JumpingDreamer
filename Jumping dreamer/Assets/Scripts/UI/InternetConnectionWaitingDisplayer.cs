using System;
using System.Collections.Generic;
using Desdiene.Singleton;
using Desdiene.Time_control;

public class InternetConnectionWaitingDisplayer : SingletonSuperMonoBehaviour<InternetConnectionWaitingDisplayer>
{
    private readonly List<object> waitingEntities = new List<object>();

    private LoadingWindow adLoadingWindow = null;


    /// <summary>
    /// Приостанавливает игровое время и выводит окно ожидания интернет соединения.
    /// </summary>
    /// <param name="waitingEntity">Ожидающая сущность. Передавать "this"</param>
    public void StartWaiting(object waitingEntity)
    {
        if (waitingEntity == null) throw new ArgumentNullException(nameof(waitingEntity));

        if (waitingEntities.Count == 0)
        {
            GlobalPause.Instance.SetInternetConnectionWaiting(true);
            EnableLoadingWindow();
        }

        waitingEntities.Add(waitingEntity);
    }

    /// <summary>
    /// Убирает ожидающую сущность из списка всех ожидающих сущностей.
    /// Если список станет пустым, то возобновляет игровое время и убирает окно ожидания интернет соединения.
    /// </summary>
    /// <param name="waitingEntity">Ожидающая сущность. Передавать "this"</param>
    public void EndWaiting(object waitingEntity)
    {
        if (waitingEntity == null) throw new ArgumentNullException(nameof(waitingEntity));

        waitingEntities.Remove(waitingEntity);

        if (waitingEntities.Count == 0)
        {
            GlobalPause.Instance.SetInternetConnectionWaiting(false);
            DisableLoadingWindow();
        }
    }


    private void EnableLoadingWindow()
    {
        adLoadingWindow = PopUpWindowGenerator.Instance.CreateLoadingWindow();
        if (adLoadingWindow == null) throw new NullReferenceException("adLoadingWindow");
    }


    private void DisableLoadingWindow()
    {
        if (adLoadingWindow == null) throw new NullReferenceException("adLoadingWindow");
        adLoadingWindow.TurnOff();
    }
}
