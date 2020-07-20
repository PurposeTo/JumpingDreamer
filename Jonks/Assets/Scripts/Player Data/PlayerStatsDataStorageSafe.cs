using System;
using UnityEngine;
using System.IO;

namespace Assets.Scripts.Player.Data
{
    public delegate void NewScoreRecord();
    public class PlayerStatsDataStorageSafe : SingletonMonoBehaviour<PlayerStatsDataStorageSafe>
    {
        public PlayerStatsDataModel PlayerStatsData { get; private set; }

        private string filePath;
        private readonly string fileName = "/Stats.json"; // Имя файла с данными (как часть всего пути)

        public event NewScoreRecord OnNewScoreRecord;

        public bool IsDataFileLoaded { get; private set; } = false;

        [SerializeField] private GameObject errorWindow = null;


        protected override void AwakeSingleton()
        {
            LoadPlayerStatsData();
        }


        private void LoadPlayerStatsData()
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                filePath = Path.Combine(Application.dataPath + fileName);
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                filePath = Path.Combine(Application.persistentDataPath + fileName);
            }

            Debug.Log($"File path: {filePath}");

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
                if (dataAsJSON != null && dataAsJSON != "")
                {
                    Debug.Log($"Data from \"{fileName}\" was loaded successfully.");
                    PlayerStatsData = JsonUtility.FromJson<PlayerStatsDataModel>(dataAsJSON);
                    IsDataFileLoaded = true;
                }
                else
                {
                    Debug.LogError($"Data reading from \"{fileName}\" ERROR!");

                    PlayerStatsData = new PlayerStatsDataModel();
                    IsDataFileLoaded = false;

                    Instantiate(errorWindow);
                    string errorMessage = "Ошибка загрузки данных игровой статистики! Запись новых данных заблокирована!";
                    errorWindow.GetComponent<ErrorWindow>().errorTextObject.text = errorMessage;
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
            }
        }


        public void SaveScoreMultiplierData(int multiplierValue)
        {
            if (IsDataFileLoaded)
            {
                if (multiplierValue > PlayerStatsData.maxScoreMultiplierValue)
                {
                    PlayerStatsData.maxScoreMultiplierValue = multiplierValue;
                }
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


#if UNITY_EDITOR

        private void OnApplicationQuit()
        {
            // А если у пользователя недостаточно памяти, чтобы создать файл?
            File.WriteAllText(filePath, JsonUtility.ToJson(PlayerStatsData));
        }

#elif UNITY_ANDROID

        private void OnApplicationPause(bool pause)
        {
            Debug.Log($"OnApplicationPause code: {pause}");
            // А если у пользователя недостаточно памяти, чтобы создать файл?
            File.WriteAllText(filePath, JsonUtility.ToJson(PlayerStatsData));
        }

#endif

    }
}
