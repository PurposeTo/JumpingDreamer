using UnityEngine;
using System;

public class GPGSLeaderboard : MonoBehaviour
{
    private Action UpdateLeaderboard;


    private void Start()
    {
        PlayerDataModelController.Instance.GetPlayerDataModel().PlayerStats.OnNewScoreRecord += SetUpdateLeaderboardMethodToAction;
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
        if (PlayerDataModelController.Instance.IsDataFileLoaded)
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
    }


    private void SetUpdateLeaderboardMethodToAction()
    {
        UpdateLeaderboard = () => UpdateLeaderboardScore(() => Social.ShowLeaderboardUI());
    }
}
