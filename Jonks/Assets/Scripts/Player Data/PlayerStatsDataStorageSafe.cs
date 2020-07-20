using System;
using UnityEngine;
using System.IO;

namespace Assets.Scripts.Player.Data
{
    public delegate void NewScoreRecord();
    public class PlayerStatsDataStorageSafe : SingletonMonoBehaviour<PlayerStatsDataStorageSafe>
    {
        [Serializable]
        public class PlayerStatsDataModel
        {
            // Лучшие результаты за все время игры
            public int maxCollectedStars;
            public int maxEarnedScore;
            public int maxPointsMultiplierValue;
            public int maxLifeTime;
            //public float maxJumpHeight; // json хранит double

            // Общие результаты за все время игры
            public int totalCollectedStars;
            public int totalLifeTime;
        }


        public PlayerStatsDataModel PlayerStatsData { get; private set; }

        private string filePath;
        private readonly string fileName = "/Stats.json"; // Имя файла с данными (как часть всего пути)

        public event NewScoreRecord OnNewScoreRecord;

        public bool IsDataFileLoaded { get; private set; } = false;


        protected override void AwakeSingleton()
        {
            LoadPlayerStatsData();
        }


        private void LoadPlayerStatsData()
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                filePath = Path.Combine(Application.dataPath + fileName);
                Debug.Log(filePath);
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                filePath = Path.Combine(Application.persistentDataPath + fileName);
                Debug.Log(filePath);
            }

            GetPlayerStatsData();
        }


        private void GetPlayerStatsData()
        {
            // Проверка на существование файла
            if (!File.Exists(filePath))
            {
                // Установить значения по дефолту
                Debug.Log($"File path \"{filePath}\" didn't found. Creating empty object...");
                PlayerStatsData = new PlayerStatsDataModel();
                IsDataFileLoaded = true;
            }
            else
            {
                Debug.Log($"File on path \"{filePath}\" was loaded.");
                string dataAsJSON = File.ReadAllText(filePath);

                // Проверка на успешность чтения данных
                if (dataAsJSON != null)
                {
                    Debug.Log($"Data from \"{fileName}\" was loaded successfully.");
                    PlayerStatsData = JsonUtility.FromJson<PlayerStatsDataModel>(dataAsJSON);
                    IsDataFileLoaded = true;
                }
                else
                {
                    Debug.LogError($"Data reading from \"{fileName}\" ERROR!");

                    // Вызвать событие ошибки, которое позволило бы ограничить запись данных в файл, чтобы не затереть старые данные?
                    // Задизеблить скрипт? - это будет неявная обработка

                    PlayerStatsData = new PlayerStatsDataModel();
                    IsDataFileLoaded = false;
                }
            }
        }


        public void SaveStarsData(int starsAmount)
        {
            if (IsDataFileLoaded)
            {
                PlayerStatsData.totalCollectedStars += starsAmount;

                if (starsAmount > PlayerStatsData.maxCollectedStars)
                {
                    PlayerStatsData.maxCollectedStars = starsAmount;
                }

                // А если у пользователя недостаточно памяти, чтобы создать файл?
                File.WriteAllText(filePath, JsonUtility.ToJson(PlayerStatsData));
            }
        }


        public void SaveScoreData(int scoreAmount)
        {
            if (IsDataFileLoaded)
            {
                if (scoreAmount > PlayerStatsData.maxEarnedScore)
                {
                    PlayerStatsData.maxEarnedScore = scoreAmount;
                    OnNewScoreRecord?.Invoke();
                }

                File.WriteAllText(filePath, JsonUtility.ToJson(PlayerStatsData));
            }
        }


        public void SaveScoreMultiplierData(int multiplierValue)
        {
            if (IsDataFileLoaded)
            {
                if (multiplierValue > PlayerStatsData.maxPointsMultiplierValue)
                {
                    PlayerStatsData.maxPointsMultiplierValue = multiplierValue;
                }

                File.WriteAllText(filePath, JsonUtility.ToJson(PlayerStatsData));
            }
        }


        public void SaveLifeTimeData(int lifeTime)
        {
            if (IsDataFileLoaded)
            {
                PlayerStatsData.totalLifeTime += lifeTime;

                if (lifeTime > PlayerStatsData.maxLifeTime)
                {
                    PlayerStatsData.maxLifeTime = lifeTime;
                }

                File.WriteAllText(filePath, JsonUtility.ToJson(PlayerStatsData));
            }
        }


        [Obsolete]
        public void SaveJumpHeightData(float jumpHeight)
        {
            //if (isDataFileLoaded)
            //{
            //    if (jumpHeight > PlayerStatsData.maxJumpHeight)
            //    {
            //        PlayerStatsData.maxJumpHeight = jumpHeight;
            //    }

            //    File.WriteAllText(filePath, JsonUtility.ToJson(PlayerStatsData));
            //}
        }
    }
}
