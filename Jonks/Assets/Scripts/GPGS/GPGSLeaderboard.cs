using UnityEngine;

public class GPGSLeaderboard : MonoBehaviour
{
    public void OpenLeaderboard()
    {
        Social.ShowLeaderboardUI();
    }


    public void UpdateLeaderboardScore()
    {
        Social.ReportScore(PlayerDataModelController.Instance.PlayerDataLocalModel.PlayerStats.MaxEarnedScore.Value, GPGSIds.leaderboard_kings_of_the_jonks, (bool success) =>
        {
            if (success)
            {
                Debug.Log("The score is load to leaderboard!");
            }
        });
    }
}
