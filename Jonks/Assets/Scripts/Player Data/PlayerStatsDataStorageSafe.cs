using System;
using UnityEngine;
using System.IO;

namespace Assets.Scripts.Player.Data
{
    public delegate void NewScoreRecord();
    public class PlayerStatsDataStorageSafe : SingletonMonoBehaviour<PlayerStatsDataStorageSafe>
    {
        public PlayerStatsDataModel PlayerStatsDataModel { get; private set; }

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
                PlayerStatsDataModel = new PlayerStatsDataModel();
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
                    PlayerStatsDataModel = JsonUtility.FromJson<PlayerStatsDataModel>(dataAsJSON);
                    IsDataFileLoaded = true;
                }
                else
                {
                    Debug.LogError($"Data reading from \"{fileName}\" ERROR!");
                    // Вывод сообщения в UI "Ошибка загрузки данных игоровой статистики! Запись новых данных заблокирована!"
                    /*
                     * 
                     */

                    // Вызвать событие ошибки, которое позволило бы ограничить запись данных в файл, чтобы не затереть старые данные?
                    // Задизеблить скрипт? - это будет неявная обработка

                    PlayerStatsDataModel = new PlayerStatsDataModel();
                }
            }
        }


        public void SaveStarsData(int starsAmount)
        {
            if (IsDataFileLoaded)
            {
                PlayerStatsDataModel.totalCollectedStarsAmount += starsAmount;

                if (starsAmount > PlayerStatsDataModel.maxCollectedStarsAmount)
                {
                    PlayerStatsDataModel.maxCollectedStarsAmount = starsAmount;
                }

                // А если у пользователя недостаточно памяти, чтобы создать файл?
                File.WriteAllText(filePath, JsonUtility.ToJson(PlayerStatsDataModel));
            }
        }


        public void SaveScoreData(int scoreAmount)
        {
            if (IsDataFileLoaded)
            {
                if (scoreAmount > PlayerStatsDataModel.maxEarnedPointsAmount)
                {
                    PlayerStatsDataModel.maxEarnedPointsAmount = scoreAmount;
                    OnNewScoreRecord?.Invoke();
                }

                File.WriteAllText(filePath, JsonUtility.ToJson(PlayerStatsDataModel));
            }
        }


        public void SaveScoreMultiplierData(int multiplierValue)
        {
            if (IsDataFileLoaded)
            {
                if (multiplierValue > PlayerStatsDataModel.maxPointsMultiplierValue)
                {
                    PlayerStatsDataModel.maxPointsMultiplierValue = multiplierValue;
                }

                File.WriteAllText(filePath, JsonUtility.ToJson(PlayerStatsDataModel));
            }
        }


        public void SaveLifeTimeData(int lifeTime)
        {
            if (IsDataFileLoaded)
            {
                PlayerStatsDataModel.totalLifeTime += lifeTime;

                if (lifeTime > PlayerStatsDataModel.maxLifeTime)
                {
                    PlayerStatsDataModel.maxLifeTime = lifeTime;
                }

                File.WriteAllText(filePath, JsonUtility.ToJson(PlayerStatsDataModel));
            }
        }


        [Obsolete]
        public void SaveJumpHeightData(float jumpHeight)
        {
            //if (isDataFileLoaded)
            //{
            //    if (jumpHeight > PlayerStatsDataModel.maxJumpHeight)
            //    {
            //        PlayerStatsDataModel.maxJumpHeight = jumpHeight;
            //    }

            //    File.WriteAllText(filePath, JsonUtility.ToJson(PlayerStatsDataModel));
            //}
        }
    }
}
