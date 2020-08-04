using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Player.Data;
using UnityEngine;

public class GPGSLeaderboard : MonoBehaviour
{
    public void OpenLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }


    public void UpdateLeaderboardScore()
    {
        Social.ReportScore(PlayerStatsDataStorageSafe.Instance.PlayerStatsData.maxEarnedScore, GPGSIds.leaderboard_kings_of_the_jonks, (bool success) =>
        {
            if (success)
            {
                Debug.Log("The score is load to leaderboard!");
            }
        });
    }
}
