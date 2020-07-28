using System;
using UnityEngine;
using System.IO;
using System.Text.Json;

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
                InitializeModel();
                IsDataFileLoaded = true;
            }
            else
            {
                Debug.Log($"File on path \"{filePath}\" was loaded.");
                //string dataAsJSON = File.ReadAllText(filePath);

                //// Проверка на успешность чтения данных (ЕСЛИ НЕ ПОЛУЧИЛОСЬ ПРЕОБРАЗОВАТЬ К МОДЕЛИ, например, стерли только часть файла -> ТОЖЕ ERROR
                //if (dataAsJSON != null && dataAsJSON != "")
                //{
                //    Debug.Log($"Data from \"{fileName}\" was loaded successfully.");
                //    PlayerStatsData = JsonUtility.FromJson<PlayerStatsDataModel>(dataAsJSON);
                //    IsDataFileLoaded = true;
                //}
                //else
                //{
                //    Debug.LogError($"Data reading from \"{fileName}\" ERROR!");

                //    PlayerStatsData = new PlayerStatsDataModel();
                //    IsDataFileLoaded = false;

                //    GameObject errorMessageObject = Instantiate(errorWindow);
                //    string errorMessage = "Ошибка загрузки данных игровой статистики! Запись новых данных заблокирована!";
                //    errorMessageObject.GetComponent<ErrorWindow>().errorTextObject.text = errorMessage;
                //}


                PlayerStatsData = JsonSerializer.Deserialize<PlayerStatsDataModel>(File.ReadAllText(filePath));
                //PlayerStatsData = JsonUtility.FromJson<PlayerStatsDataModel>(File.ReadAllText(filePath));

                // Если в файле присутствуют поля со значением null (т.е. если файл изменялся), то сообщить об ошибке
                if (!TrySetDefaultValues())
                {
                    Debug.Log($"Data from \"{fileName}\" was loaded successfully.");
                    IsDataFileLoaded = true;
                }
                else
                {
                    Debug.LogError($"Data reading from \"{fileName}\" ERROR!");
                    IsDataFileLoaded = false;

                    Instantiate(errorWindow);
                    string errorMessage = "Ошибка загрузки данных игровой статистики! Запись новых данных заблокирована!";
                    errorWindow.GetComponent<ErrorWindow>().errorTextObject.text = errorMessage;
                }
            }
        }


        private void InitializeModel()
        {
            PlayerStatsData = new PlayerStatsDataModel
            {
                maxCollectedStars = default(int),
                maxEarnedScore = default(int),
                maxScoreMultiplierValue = default(int),
                maxLifeTime = default(int),
                totalCollectedStars = default(int),
                totalLifeTime = default(int),
            };
        }


        // Проверить все значения модели на null, а затем вернуть флаг (изменяли ли модель или нет). Установить все null values И ТОЛЬКО ИХ! в default для того, чтобы можно было продолжить работу с ними
        private bool TrySetDefaultValues()
        {
            bool haveNullValue = false;

            //PlayerStatsData.maxCollectedStars ??= default;
            //PlayerStatsData.maxEarnedScore ??= default;
            //PlayerStatsData.maxScoreMultiplierValue ??= default;
            //PlayerStatsData.maxLifeTime ??= default;
            //PlayerStatsData.totalCollectedStars ??= default;
            //PlayerStatsData.totalLifeTime ??= default;

            if (!PlayerStatsData.maxCollectedStars.HasValue)
            {
                PlayerStatsData.maxCollectedStars = default(int);
                haveNullValue = true;
            }
            if (!PlayerStatsData.maxEarnedScore.HasValue)
            {
                PlayerStatsData.maxEarnedScore = default(int);
                haveNullValue = true;
            }
            if (!PlayerStatsData.maxScoreMultiplierValue.HasValue)
            {
                PlayerStatsData.maxScoreMultiplierValue = default(int);
                haveNullValue = true;
            }
            if (!PlayerStatsData.maxLifeTime.HasValue)
            {
                PlayerStatsData.maxLifeTime = default(int);
                haveNullValue = true;
            }
            if (!PlayerStatsData.totalCollectedStars.HasValue)
            {
                PlayerStatsData.totalCollectedStars = default(int);
                haveNullValue = true;
            }
            if (!PlayerStatsData.totalLifeTime.HasValue)
            {
                PlayerStatsData.totalLifeTime = default(int);
                haveNullValue = true;
            }

            return haveNullValue;
        }


        public void SaveStarsData(int starsAmount)
        {
            PlayerStatsData.totalCollectedStars += starsAmount;

            if (starsAmount > PlayerStatsData.maxCollectedStars)
            {
                PlayerStatsData.maxCollectedStars = starsAmount;
            }
        }


        public void SaveScoreData(int scoreAmount)
        {
            if (scoreAmount > PlayerStatsData.maxEarnedScore)
            {
                PlayerStatsData.maxEarnedScore = scoreAmount;
                OnNewScoreRecord?.Invoke();
            }
        }


        public void SaveScoreMultiplierData(int multiplierValue)
        {
            if (multiplierValue > PlayerStatsData.maxScoreMultiplierValue)
            {
                PlayerStatsData.maxScoreMultiplierValue = multiplierValue;
            }
        }


        public void SaveLifeTimeData(int lifeTime)
        {
            PlayerStatsData.totalLifeTime += lifeTime;

            if (lifeTime > PlayerStatsData.maxLifeTime)
            {
                PlayerStatsData.maxLifeTime = lifeTime;
            }
        }


        [Obsolete]
        public void SaveJumpHeightData(float jumpHeight)
        {
            //if (jumpHeight > PlayerStatsData.maxJumpHeight)
            //{
            //    PlayerStatsData.maxJumpHeight = jumpHeight;
            //}
        }


        private void WritePlayerDataToFile()
        {
            if (IsDataFileLoaded)
            {
                // А если у пользователя недостаточно памяти, чтобы создать файл?
                File.WriteAllText(filePath, JsonSerializer.Serialize(PlayerStatsData));
                //File.WriteAllText(filePath, JsonUtility.ToJson(PlayerStatsData));
            }
        }


#if UNITY_EDITOR

        private void OnApplicationQuit()
        {
            WritePlayerDataToFile();
        }

#elif UNITY_ANDROID

        private void OnApplicationPause(bool pause)
        {
            Debug.Log($"OnApplicationPause code: {pause}");
            WritePlayerDataToFile();
        }

#endif

    }
}
