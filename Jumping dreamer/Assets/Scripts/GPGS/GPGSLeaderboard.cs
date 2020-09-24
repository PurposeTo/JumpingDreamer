using UnityEngine;

public class GPGSLeaderboard : SingletonMonoBehaviour<GPGSLeaderboard>
{
    public void OpenLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }


    public void UpdateLeaderboardScore()
    {
        Social.ReportScore(PlayerDataModelController.Instance.PlayerDataLocalModel.PlayerStats.MaxEarnedScore.Value, GPGSIds.leaderboard_dreamer_the_king, (bool success) =>
        {
            if (success)
            {
                Debug.Log("The score is load to leaderboard!");
            }
        });
    }
}
