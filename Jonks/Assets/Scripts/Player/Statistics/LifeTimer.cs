using Assets.Scripts.Player.Data;
using UnityEngine;
using System;

namespace Assets.Scripts
{
    public class LifeTimer : MonoBehaviour
    {
        private float lifeTime = 0f;


        private void Start()
        {
            GameMenu.Instance.GameOverScreen.GameOverStatusScreen.OnSavePlayerStats += SaveLifeTimeStats;
        }


        private void OnDestroy()
        {
            GameMenu.Instance.GameOverScreen.GameOverStatusScreen.OnSavePlayerStats -= SaveLifeTimeStats;
        }


        private void Update()
        {
            lifeTime += 1f * Time.deltaTime;
        }


        private void SaveLifeTimeStats()
        {
            PlayerStatsDataStorageSafe.Instance.SaveLifeTimeData((int)lifeTime);
            lifeTime = 0f;
        }
    }
}
