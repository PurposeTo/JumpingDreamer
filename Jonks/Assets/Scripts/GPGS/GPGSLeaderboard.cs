﻿using System.Collections;
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
        Social.ReportScore(PlayerStatsDataStorageSafe.Instance.PlayerStatsDataModel.maxEarnedPointsAmount, GPGSIds.leaderboard_king_of_the_jonks, (bool success) =>
        {
            if (success)
            {
                Debug.Log("Score is load to leaderboard!");
            }
        });
    }
}
