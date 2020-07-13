using System;
using System.Collections;
using UnityEngine;
using System.IO;
using Assets.Scripts.Player.DataModel;

namespace Assets.Scripts.Player.Data
{
    public class PlayerStatsDataStorageSafe : SingletonMonoBehaviour<PlayerStatsDataStorageSafe>
    {
        public PlayerStatsDataModel PlayerStatsDataModel { get; private set; }

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
                PlayerStatsDataModel = GetPlayerStatsDataOnEditor();
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
            PlayerStatsDataModel = JsonUtility.FromJson<PlayerStatsDataModel>(dataAsJson);

            Debug.Log("Load Player Stats Data is done on Android");
        }


        public void SaveStarsData(int starsAmount)
        {
            PlayerStatsDataModel.totalCollectedStarsAmount += starsAmount;

            if (starsAmount > PlayerStatsDataModel.maxCollectedStarsAmount)
            {
                PlayerStatsDataModel.maxCollectedStarsAmount = starsAmount;
            }

            File.WriteAllText(filePath, JsonUtility.ToJson(PlayerStatsDataModel));
        }


        public void SaveScoreData(int scoreAmount)
        {
            if (scoreAmount > PlayerStatsDataModel.maxEarnedPointsAmount)
            {
                PlayerStatsDataModel.maxEarnedPointsAmount = scoreAmount;
            }

            File.WriteAllText(filePath, JsonUtility.ToJson(PlayerStatsDataModel));
        }


        public void SaveScoreMultiplierData(int multiplierValue)
        {
            if (multiplierValue > PlayerStatsDataModel.maxPointsMultiplierValue)
            {
                PlayerStatsDataModel.maxPointsMultiplierValue = multiplierValue;
            }

            File.WriteAllText(filePath, JsonUtility.ToJson(PlayerStatsDataModel));
        }


        public void SaveLifeTimeData(int lifeTime)
        {
            //playerStatsDataModel.totalLifeTime += TimeSpan.FromSeconds(lifeTime);
            PlayerStatsDataModel.totalLifeTime += lifeTime;

            if (lifeTime > PlayerStatsDataModel.maxLifeTime)
            {
                PlayerStatsDataModel.maxLifeTime = lifeTime;
            }

            File.WriteAllText(filePath, JsonUtility.ToJson(PlayerStatsDataModel));
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
