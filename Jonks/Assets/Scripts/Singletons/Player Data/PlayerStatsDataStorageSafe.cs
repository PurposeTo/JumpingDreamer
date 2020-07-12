using UnityEngine;
using System.IO;
using Assets.Scripts.Player.DataModel;
using System;

namespace Assets.Scripts.Player.Data
{
    public class PlayerStatsDataStorageSafe : SingletonMonoBehaviour<PlayerStatsDataStorageSafe>
    {
        private PlayerStatsDataModel playerStatsDataModel;

        private string filePath;
        private readonly string fileName = "Stats.json"; // Имя файла с данными (как часть всего пути)


        protected override void AwakeSingleton()
        {
            playerStatsDataModel = GetPlayerStatsData();
        }


        // Определение пути файла с данными
        private void Start()
        {
#if UNITY_ANDROID
            filePath = Path.Combine(Application.persistentDataPath, fileName);
#else
            filePath = Path.Combine(Application.dataPath, fileName);
#endif
        }


        public PlayerStatsDataModel GetPlayerStatsData()
        {
            if (!File.Exists(filePath))
            {
                // Вернуть значения по дефолту
                return new PlayerStatsDataModel();
            }

            return JsonUtility.FromJson<PlayerStatsDataModel>(filePath);
        }


        public void SaveCoinsData(int coinsAmount)
        {
            playerStatsDataModel.totalCollectedCoinsAmount += coinsAmount;

            if (coinsAmount > playerStatsDataModel.maxCollectedCoinsAmount)
            {
                playerStatsDataModel.maxCollectedCoinsAmount = coinsAmount;
            }

            File.WriteAllText(filePath, JsonUtility.ToJson(playerStatsDataModel));
        }


        public void SaveScoreData(int scoreAmount)
        {
            if (scoreAmount > playerStatsDataModel.maxEarnedPointsAmount)
            {
                playerStatsDataModel.maxEarnedPointsAmount = scoreAmount;
            }

            File.WriteAllText(filePath, JsonUtility.ToJson(playerStatsDataModel));
        }


        public void SaveScoreMultiplierData(int multiplierValue)
        {
            if (multiplierValue > playerStatsDataModel.maxPointsMultiplierValue)
            {
                playerStatsDataModel.maxPointsMultiplierValue = multiplierValue;
            }

            File.WriteAllText(filePath, JsonUtility.ToJson(playerStatsDataModel));
        }


        public void SaveTotalLifeTimeData(float gameTime)
        {
            playerStatsDataModel.totalLifeTime += TimeSpan.FromSeconds(gameTime);
        }


        public void SaveMaxLifeTimeData(float gameTime)
        {
            if (gameTime > playerStatsDataModel.maxLifeTime)
            {
                playerStatsDataModel.maxLifeTime = gameTime;
            }

            File.WriteAllText(filePath, JsonUtility.ToJson(playerStatsDataModel));
        }


        public void SaveJumpHeightData(float jumpHeight)
        {
            if (jumpHeight > playerStatsDataModel.maxJumpHeight)
            {
                playerStatsDataModel.maxJumpHeight = jumpHeight;
            }

            File.WriteAllText(filePath, JsonUtility.ToJson(playerStatsDataModel));
        }
    }
}
