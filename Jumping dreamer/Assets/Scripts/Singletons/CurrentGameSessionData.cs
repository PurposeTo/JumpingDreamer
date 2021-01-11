using System;

public class CurrentGameSessionData : SingletonSuperMonoBehaviour<CurrentGameSessionData>
{
    public bool IsRecordNew { get; private set; } = false;

    public event Action OnNewRecordScore;
    public event Action OnScoreOrStarsChanged;

    protected override void StartWrapped()
    {
        PlayerDataModelController.Instance.DataInteraction.Notifier.StatsChangingNotifier.OnNewScoreRecord += ActivateRecordNewToggle;
    }

    protected override void OnDestroyWrapped()
    {
        PlayerDataModelController.Instance.DataInteraction.Notifier.StatsChangingNotifier.OnNewScoreRecord -= ActivateRecordNewToggle;
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
