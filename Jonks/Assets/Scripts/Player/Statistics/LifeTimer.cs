using Assets.Scripts.Player.Data;
using UnityEngine;
using System;

namespace Assets.Scripts
{
    public class LifeTimer : MonoBehaviour
    {
        public float lifeTime => Time.time;


        private void Start()
        {
            GameMenu.Instance.GameOverScreen.GameOverStatusScreen.OnSavePlayerStats += SaveLifeTimeStats;
        }


        private void OnDestroy()
        {
            GameMenu.Instance.GameOverScreen.GameOverStatusScreen.OnSavePlayerStats -= SaveLifeTimeStats;
        }


        private void SaveLifeTimeStats()
        {
            PlayerStatsDataStorageSafe.Instance.SaveLifeTimeData((float)Math.Round(lifeTime, 1));
        }
    }
}
