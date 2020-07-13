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
            LoadPlayerStatsData();
        }


        private void LoadPlayerStatsData()
        {
            GetFilePath();

            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                playerStatsDataModel = GetPlayerStatsDataOnEditor();
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                StartCoroutine(GetPlayerStatsDataOnAndroid());
            }
        }


        private void GetFilePath()
        {
            filePath = Path.Combine(Application.streamingAssetsPath, "PlayerStatsData/" + fileName);
            Debug.Log(filePath);
        }


        public PlayerStatsDataModel GetPlayerStatsDataOnEditor()
        {
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

            if (reader.error != null)
            {
                Debug.LogError(reader.error);
                yield break;
            }

            string dataAsJson = reader.text;
            playerStatsDataModel = JsonUtility.FromJson<PlayerStatsDataModel>(dataAsJson);

            Debug.Log("Load Player Stats Data is done on Android");
        }


        public void SaveStarsData(int starsAmount)
        {
            playerStatsDataModel.totalCollectedStarsAmount += starsAmount;

            if (starsAmount > playerStatsDataModel.maxCollectedStarsAmount)
            {
                playerStatsDataModel.maxCollectedStarsAmount = starsAmount;
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


        public void SaveLifeTimeData(int lifeTime)
        {
            //playerStatsDataModel.totalLifeTime += TimeSpan.FromSeconds(lifeTime);
            playerStatsDataModel.totalLifeTime += lifeTime;

            if (lifeTime > playerStatsDataModel.maxLifeTime)
            {
                playerStatsDataModel.maxLifeTime = lifeTime;
            }

            File.WriteAllText(filePath, JsonUtility.ToJson(playerStatsDataModel));
        }


        [Obsolete]
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
