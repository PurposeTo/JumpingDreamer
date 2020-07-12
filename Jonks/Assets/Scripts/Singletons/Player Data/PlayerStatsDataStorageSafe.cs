using System;
using System.Collections;
using UnityEngine;
using System.IO;
using Assets.Scripts.Player.DataModel;

namespace Assets.Scripts.Player.Data
{
    public class PlayerStatsDataStorageSafe : SingletonMonoBehaviour<PlayerStatsDataStorageSafe>
    {
        private PlayerStatsDataModel playerStatsDataModel;

        private string filePath;
        private readonly string fileName = "Stats.json"; // Имя файла с данными (как часть всего пути)


        protected override void AwakeSingleton()
        {

            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                playerStatsDataModel = GetPlayerStatsData();
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                StartCoroutine(GetPlayerStatsDataOnAndroid());
            }
        }


        private void GetFilePath()
        {
            filePath = Path.Combine(Application.streamingAssetsPath, "PlayerStatsData/" + fileName);
            print(filePath);
        }


        public PlayerStatsDataModel GetPlayerStatsData()
        {
            GetFilePath();

            if (!File.Exists(filePath))
            {
                // Вернуть значения по дефолту
                Debug.Log($"File {fileName} didn't found. Creating empty object...");
                return new PlayerStatsDataModel();
            }
            else
            {
                Debug.Log($"File {fileName} was loaded.");
            }

            string dataAsJSON = File.ReadAllText(filePath);
            return JsonUtility.FromJson<PlayerStatsDataModel>(dataAsJSON);
        }


        private IEnumerator GetPlayerStatsDataOnAndroid()
        {
            WWW reader = new WWW(filePath);
            yield return reader;
            Debug.LogWarning(filePath);
            if (reader.error != null)
            {
                Debug.LogWarning(reader.error);
                yield break;
            }

            string dataAsJson = reader.text;

            playerStatsDataModel = JsonUtility.FromJson<PlayerStatsDataModel>(dataAsJson);

            Debug.Log("Load Player Stats Data is done on Android");
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


        public void SaveLifeTimeData(float lifeTime)
        {
            //playerStatsDataModel.totalLifeTime += TimeSpan.FromSeconds(lifeTime);
            playerStatsDataModel.totalLifeTime += lifeTime;

            if (lifeTime > playerStatsDataModel.maxLifeTime)
            {
                playerStatsDataModel.maxLifeTime = lifeTime;
            }

            File.WriteAllText(filePath, JsonUtility.ToJson(playerStatsDataModel));
        }


        public void SaveJumpHeightData(float jumpHeight)
        {
            //if (jumpHeight > playerStatsDataModel.maxJumpHeight)
            //{
            //    playerStatsDataModel.maxJumpHeight = jumpHeight;
            //}

            //File.WriteAllText(filePath, JsonUtility.ToJson(playerStatsDataModel));
        }
    }
}
