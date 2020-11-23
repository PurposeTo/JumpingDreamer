using UnityEngine;
using System;

public class GPGSLeaderboard : SingletonSuperMonoBehaviour<GPGSLeaderboard>
{
    private Action UpdateLeaderboard;

    protected override void StartWrapped()
    {
        PlayerDataModelController.Instance.OnPlayerDataModelAvailable += (playerDataModel) =>
        {
            playerDataModel.PlayerStats.OnNewScoreRecord += SetUpdateLeaderboardMethodToAction;
        };
    }


    private void OnDestroy()
    {
        PlayerDataModelController.Instance.GetPlayerDataModel().PlayerStats.OnNewScoreRecord -= SetUpdateLeaderboardMethodToAction;
    }


    public void OpenLeaderboard()
    {
        UpdateLeaderboard?.Invoke();
        if (UpdateLeaderboard == null) Social.ShowLeaderboardUI();
    }


    public void UpdateLeaderboardScore(Action openLeaderboardAction)
    {
        Social.ReportScore(PlayerDataModelController.Instance.GetPlayerDataModel().PlayerStats.MaxEarnedScore.Value, GPGSIds.leaderboard_dreamer_the_king, (bool success) =>
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
