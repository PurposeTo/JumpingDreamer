using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayerOfLoading : SingletonSuperMonoBehaviour<DisplayerOfLoading>
{
    private readonly List<object> waitingEntities = new List<object>();
    private Pauser pauser;

    private LoadingWindow adLoadingWindow = null;

    protected override void AwakeSingleton()
    {
        pauser = new Pauser(this);
    }


    /// <summary>
    /// Приостанавливает игровое время и выводит loading window на период ожидания.
    /// </summary>
    /// <param name="waitingEntity">Ожидающая сущность. Передавать "this"</param>
    public void StartWaiting(object waitingEntity)
    {
        if (waitingEntity is null) throw new ArgumentNullException(nameof(waitingEntity));

        if (waitingEntities.Count == 0)
        {
            pauser.SetPause(true);
            EnableLoadingWindow();
        }

        waitingEntities.Add(waitingEntity);
    }

    /// <summary>
    /// Убирает ожидающую сущность из списка всех ожидающих сущностей.
    /// Если список станет пустым, то возобновляет игровое время и выключает loading window.
    /// </summary>
    /// <param name="waitingEntity">Ожидающая сущность. Передавать "this"</param>
    public void EndWaiting(object waitingEntity)
    {
        if (waitingEntity is null) throw new ArgumentNullException(nameof(waitingEntity));

        waitingEntities.Remove(waitingEntity);

        if (waitingEntities.Count == 0)
        {
            pauser.SetPause(false);
            DisableLoadingWindow();
        }
    }


    private void EnableLoadingWindow()
    {
        adLoadingWindow = PopUpWindowGenerator.Instance.CreateLoadingWindow();
        if (adLoadingWindow is null) throw new NullReferenceException("adLoadingWindow");
    }


    private void DisableLoadingWindow()
    {
        if (adLoadingWindow is null) throw new NullReferenceException("adLoadingWindow");
        adLoadingWindow.TurnOff();
    }
}
