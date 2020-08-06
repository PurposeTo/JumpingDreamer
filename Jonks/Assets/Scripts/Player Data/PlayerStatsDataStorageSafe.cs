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

        public string FilePath { get; private set; }
        private readonly string fileName = "Stats.json"; // Имя файла с данными (как часть всего пути)

        public event NewScoreRecord OnNewScoreRecord;

        public bool IsDataFileLoaded { get; private set; } = false;

        [SerializeField] private GameObject errorWindow = null;


        protected override void AwakeSingleton()
        {
            LoadPlayerStatsData();
        }


        private void Start()
        {
            MainMenu.Instance.SettingsMenu.ResetStatsButton.ConfirmationDeleteWindow.OnDeleteStats += DeletePlayerStatsData;
        }


        private void OnDestroy()
        {
            MainMenu.Instance.SettingsMenu.ResetStatsButton.ConfirmationDeleteWindow.OnDeleteStats -= DeletePlayerStatsData;
        }


        private void LoadPlayerStatsData()
        {
            FilePath = DataLoaderHelper.GetFilePath(fileName);
            Debug.Log($"File path: {FilePath}");

            GetPlayerStatsData();
        }


        private void GetPlayerStatsData()
        {
            // Проверка на существование файла
            if (!File.Exists(FilePath))
            {
                // Установить значения по дефолту
                Debug.Log($"File path \"{FilePath}\" didn't found. Creating empty object...");

                PlayerStatsData = PlayerStatsDataModel.CreateModelWithDefaultValues();
                IsDataFileLoaded = true;
            }
            else
            {
                Debug.Log($"File on path \"{FilePath}\" was found.");

                string dataAsJSON = JsonEncryption.Decrypt(FilePath);
                if (dataAsJSON != null)
                {
                    PlayerStatsData = JsonSerializer.Deserialize<PlayerStatsDataModel>(dataAsJSON);
                    IsDataFileLoaded = true;

                    Debug.Log($"Data from \"{fileName}\" was loaded successfully.");
                    print($"###: maxStars: {PlayerStatsData.MaxCollectedStars} \t maxScore: {PlayerStatsData.MaxEarnedScore} \t maxLifeTime: { PlayerStatsData.MaxLifeTime} \t totalStars: {PlayerStatsData.TotalCollectedStars}");
                }
                else
                {
                    PlayerStatsData = PlayerStatsDataModel.CreateModelWithDefaultValues();
                    IsDataFileLoaded = false;
                    CreateErrorWindow();

                    Debug.LogError($"Data reading from \"{fileName}\" ERROR!");
                }
                //// Если не получилось сконверитровать файл ИЛИ полностью подтерли поле внутри файла, то сообщить об ошибке
                //if (!TryReadFile())
                //{
                //    Debug.LogError($"Data reading from \"{fileName}\" ERROR!");

                //    IsDataFileLoaded = false;
                //    CreateErrorWindow();
                //}
                //else
                //{
                //    Debug.Log($"Data from \"{fileName}\" was loaded successfully.");

                //    IsDataFileLoaded = true;
                //}
            }
        }


        //private bool TryReadFile()
        //{
        //    bool okConverted;
        //    bool isJsonWasEdited;

        //    try
        //    {
        //        PlayerStatsData = JsonSerializer.Deserialize<PlayerStatsDataModel>(File.ReadAllText(FilePath));
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.LogError(ex.Message);

        //        okConverted = false;
        //        return okConverted;
        //    }

        //    isJsonWasEdited = PlayerStatsData.TrySetDefaultValues();
        //    return !isJsonWasEdited;
        //}


        // Попробовать убрать из этого класса
        private void CreateErrorWindow()
        {
            Instantiate(errorWindow);
            string errorMessage = "Ошибка загрузки данных игровой статистики! Запись новых данных заблокирована!";
            errorWindow.GetComponent<ErrorWindow>().errorTextObject.text = errorMessage;
        }


        private void DeletePlayerStatsData(object sender, EventArgs eventArgs)
        {
            PlayerStatsData = PlayerStatsDataModel.CreateModelWithDefaultValues();
            File.Delete(FilePath);
            File.Delete(FilePath + ".meta");
            File.Delete(JsonEncryption.filePath);
            File.Delete(JsonEncryption.filePath + ".meta");
            IsDataFileLoaded = true; // Снова можем записывать информацию в файл
        }


        public void SaveStarsData(SafeInt starsAmount)
        {
            PlayerStatsData.TotalCollectedStars += starsAmount;

            if (starsAmount > PlayerStatsData.MaxCollectedStars)
            {
                PlayerStatsData.MaxCollectedStars = starsAmount;
            }
        }


        public void SaveScoreData(SafeInt scoreAmount)
        {
            if (scoreAmount > PlayerStatsData.MaxEarnedScore)
            {
                PlayerStatsData.MaxEarnedScore = scoreAmount;
                OnNewScoreRecord?.Invoke();
            }
        }


        public void SaveScoreMultiplierData(SafeInt multiplierValue)
        {
            if (multiplierValue > PlayerStatsData.MaxScoreMultiplierValue)
            {
                PlayerStatsData.MaxScoreMultiplierValue = multiplierValue;
            }
        }


        public void SaveLifeTimeData(SafeInt lifeTime)
        {
            PlayerStatsData.TotalLifeTime += lifeTime;

            if (lifeTime > PlayerStatsData.MaxLifeTime)
            {
                PlayerStatsData.MaxLifeTime = lifeTime;
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
                string json = JsonSerializer.Serialize(PlayerStatsData);
                string modifiedData = JsonEncryption.Encrypt(json);
                File.WriteAllText(FilePath, modifiedData);
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
