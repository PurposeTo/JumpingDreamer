using UnityEngine;
using System;

public class GPGSLeaderboard : SingletonMonoBehaviour<GPGSLeaderboard>
{
    private Action UpdateLeaderboard;


    private void Start()
    {
        PlayerDataModelController.Instance.PlayerDataLocalModel.PlayerStats.OnNewScoreRecord += SetUpdateLeaderboardMethodToAction;
    }


    private void OnDestroy()
    {
        PlayerDataModelController.Instance.PlayerDataLocalModel.PlayerStats.OnNewScoreRecord -= SetUpdateLeaderboardMethodToAction;
    }


    public void OpenLeaderboard()
    {
        UpdateLeaderboard?.Invoke();
        if (UpdateLeaderboard == null) Social.ShowLeaderboardUI();
    }


    public void UpdateLeaderboardScore(Action openLeaderboardAction)
    {
        if (PlayerDataModelController.Instance.IsDataFileLoaded)
        {
            Social.ReportScore(PlayerDataModelController.Instance.PlayerDataLocalModel.PlayerStats.MaxEarnedScore.Value, GPGSIds.leaderboard_kings_of_the_jonks, (bool success) =>
            {
                if (success)
                {
                    Debug.Log("The score is load to leaderboard!");
                    openLeaderboardAction?.Invoke();
                    UpdateLeaderboard = null;
                }
            });
        }
    }


    private void SetUpdateLeaderboardMethodToAction()
    {
        UpdateLeaderboard = () => UpdateLeaderboardScore(() => Social.ShowLeaderboardUI());
    }
}
