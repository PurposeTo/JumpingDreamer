using UnityEngine;
using System;

public class GPGSLeaderboard : SingletonSuperMonoBehaviour<GPGSLeaderboard>
{
    private Action UpdateLeaderboard;

    protected override void StartWrapped()
    {
        PlayerDataModelController.Instance.GetGettableDataModel().PlayerStats.OnNewScoreRecord += SetUpdateLeaderboardMethodToAction;
    }


    protected override void OnDestroyWrapped()
    {
        PlayerDataModelController.Instance.GetGettableDataModel().PlayerStats.OnNewScoreRecord -= SetUpdateLeaderboardMethodToAction;
    }


    public void OpenLeaderboard()
    {
        UpdateLeaderboard?.Invoke();
        if (UpdateLeaderboard == null) Social.ShowLeaderboardUI();
    }


    public void UpdateLeaderboardScore(Action openLeaderboardAction)
    {
        Social.ReportScore(PlayerDataModelController.Instance.GetGettableDataModel().PlayerStats.MaxEarnedScore.Value, GPGSIds.leaderboard_dreamer_the_king, (bool success) =>
        {
            if (success)
            {
                Debug.Log("The score is load to leaderboard!");
                openLeaderboardAction?.Invoke();
                UpdateLeaderboard = null;
            }
        });
    }


    private void SetUpdateLeaderboardMethodToAction()
    {
        UpdateLeaderboard = () => UpdateLeaderboardScore(() => Social.ShowLeaderboardUI());
    }
}
