﻿using System;

public class CurrentGameSessionData : SingletonMonoBehaviour<CurrentGameSessionData>
{
    public bool IsRecordNew { get; private set; } = false;

    public event Action OnNewRecordScore;
    public event Action OnScoreOrStarsChanged;

    private void Start()
    {
        PlayerDataModelController.Instance.GetPlayerDataModel().PlayerStats.OnNewScoreRecord += ActivateRecordNewToggle;
    }

    private void OnDestroy()
    {
        PlayerDataModelController.Instance.GetPlayerDataModel().PlayerStats.OnNewScoreRecord -= ActivateRecordNewToggle;
    }


    private void ActivateRecordNewToggle()
    {
        IsRecordNew = true;
        OnNewRecordScoreCall();
    }


    private void OnScoreOrStarsChangedCall()
    {
        OnScoreOrStarsChanged?.Invoke();
    }


    private void OnNewRecordScoreCall()
    {
        OnNewRecordScore?.Invoke();
    }
}