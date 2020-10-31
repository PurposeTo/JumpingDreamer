using System;

public class CurrentSessionData : SingletonMonoBehaviour<CurrentSessionData>
{
    public bool IsRecordNew { get; private set; } = false;

    public Action OnNewRecordScore;
    public Action OnScoreOrStarsChanged;

    private void Start()
    {
        PlayerDataModelController.Instance.GetPlayerDataModel().PlayerStats.OnNewScoreRecord += ActivateRecordNewToggle;
        GameManager.Instance.PlayerPresenter.ScoreCollector.OnScoreAmountChange += OnScoreOrStarsChangedCall;
        GameManager.Instance.PlayerPresenter.StarCollector.OnStarAmountChange += OnScoreOrStarsChangedCall;
    }

    private void OnDestroy()
    {
        PlayerDataModelController.Instance.GetPlayerDataModel().PlayerStats.OnNewScoreRecord -= ActivateRecordNewToggle;
        GameManager.Instance.PlayerPresenter.ScoreCollector.OnScoreAmountChange -= OnScoreOrStarsChangedCall;
        GameManager.Instance.PlayerPresenter.StarCollector.OnStarAmountChange -= OnScoreOrStarsChangedCall;
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
