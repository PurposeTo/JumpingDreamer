using Assets.Scripts.Player.Data;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameTimer : MonoBehaviour
    {
        public float gameTime => Time.time;


        private void Start()
        {
            GameMenu.Instance.GameOverScreen.GameOverStatusScreen.OnSavePlayerStats += SaveGameTimeStats;
        }


        private void OnDestroy()
        {
            GameMenu.Instance.GameOverScreen.GameOverStatusScreen.OnSavePlayerStats -= SaveGameTimeStats;
        }


        private void SaveGameTimeStats()
        {
            PlayerStatsDataStorageSafe.Instance.SaveTotalLifeTimeData(gameTime);
            PlayerStatsDataStorageSafe.Instance.SaveMaxLifeTimeData(gameTime);
        }
    }
}
