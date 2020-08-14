using System;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class PlayerStatsDataStorageSafe : SingletonMonoBehaviour<PlayerStatsDataStorageSafe>
{
    public PlayerStatsDataModel PlayerStatsData { get; private set; }
    private JsonSerializerSettings serializerSettings;

    public string FilePath { get; private set; }
    private readonly string fileName = "Stats.json"; // Имя файла с данными (как часть всего пути)

    public bool IsDataFileLoaded { get; private set; } = false;

    public event EventHandler OnNewScoreRecord;


    protected override void AwakeSingleton()
    {
        LoadPlayerStatsData();
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
            if (pause)
            {
                WritePlayerDataToFile();
            }
        }

#endif


    private void LoadPlayerStatsData()
    {
        void SetSerializerSettings()
        {
            serializerSettings = new JsonSerializerSettings();
            serializerSettings.Converters.Add(new SafeIntConverter());
        }


        FilePath = DataLoaderHelper.GetFilePath(fileName);
        Debug.Log($"File path: {FilePath}");

        SetSerializerSettings();

        GetPlayerStatsData();
    }


    private void GetPlayerStatsData()
    {
        // Проверка на существование файла
        if (File.Exists(FilePath))
        {
            Debug.Log($"File on path \"{FilePath}\" was found.");

            string dataAsJSON = JsonEncryption.Decrypt(FilePath);
            bool isJsonConverted = true;
            bool isJsonStructureIncorrect = false;

            // Если не было внешнего вмешательства в файлы с данными
            if (dataAsJSON != null)
            {
                try
                {
                    PlayerStatsData = JsonConvert.DeserializeObject<PlayerStatsDataModel>(dataAsJSON, serializerSettings);
                    print("#MaxCollectedStars: " + PlayerStatsData.MaxCollectedStars);
                    print("#MaxEarnedScore: " + PlayerStatsData.MaxEarnedScore);
                    print("#MaxLifeTime: " + PlayerStatsData.MaxLifeTime);

                    // Структура json соответствует модели данных?
                    isJsonStructureIncorrect = PlayerStatsData.IsModelHasNullValues();
                }
                catch (Exception exception)
                {
                    isJsonConverted = false;

                    Debug.LogError($"Unsuccessful attempt of deserialization: {exception.Message}");
                    ErrorWindowGenerator.Instance.CreateErrorWindow($"{exception.Message}\nОшибка загрузки данных игровой статистики!\nЗапись новых данных заблокирована!");
                }
            }
            
            if(dataAsJSON == null || isJsonStructureIncorrect || !isJsonConverted)
            {
                PlayerStatsData = PlayerStatsDataModel.CreateModelWithDefaultValues();
                print("#MaxCollectedStars: " + PlayerStatsData.MaxCollectedStars);
                print("#MaxEarnedScore: " + PlayerStatsData.MaxEarnedScore);
                print("#MaxLifeTime: " + PlayerStatsData.MaxLifeTime);
                IsDataFileLoaded = false;

                Debug.LogError($"Data reading from \"{fileName}\" ERROR!\nData was edited from outside.");
                Debug.LogError($"dataAsJSON: {dataAsJSON}");
                Debug.LogError($"isJsonStructureIncorrect: {isJsonStructureIncorrect}");
                Debug.LogError($"isJsonConverted: {isJsonConverted}");
                ErrorWindowGenerator.Instance.CreateErrorWindow("Ошибка загрузки данных игровой статистики!\nЗапись новых данных заблокирована!");
            }
            else
            {
                IsDataFileLoaded = true;
                Debug.Log($"Data from \"{fileName}\" was loaded successfully.");
            }
        }
        else
        {
            // Установить значения по дефолту
            Debug.Log($"File path \"{FilePath}\" didn't found. Creating empty object...");

            PlayerStatsData = PlayerStatsDataModel.CreateModelWithDefaultValues();
            print("#MaxCollectedStars: " + PlayerStatsData.MaxCollectedStars);
            print("#MaxEarnedScore: " + PlayerStatsData.MaxEarnedScore);
            print("#MaxLifeTime: " + PlayerStatsData.MaxLifeTime);
            IsDataFileLoaded = true;
        }
    }


    public void DeletePlayerStatsData()
    {
        PlayerStatsData = PlayerStatsDataModel.CreateModelWithDefaultValues();
        File.Delete(FilePath);
        File.Delete(FilePath + ".meta");
        File.Delete(JsonEncryption.FilePathWithHash);
        File.Delete(JsonEncryption.FilePathWithHash + ".meta");
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
            OnNewScoreRecord?.Invoke(this, null);
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
            print($"#PlayerStatsData: {PlayerStatsData}");
            print($"#PlayerStatsData.MaxEarnedScore: {PlayerStatsData.MaxEarnedScore}");

            string json = "";
            bool isJsonConverted = true;
            try
            {
                json = JsonConvert.SerializeObject(PlayerStatsData, serializerSettings);
            }
            catch (Exception exception)
            {
                isJsonConverted = false;

                Debug.LogError($"Unsuccessful attempt of serialization: {exception.Message}");
                ErrorWindowGenerator.Instance.CreateErrorWindow("Ошибка записи данных игровой статистики! Пожалуйста, обратитесь в службу поддержки.");
            }

            if (isJsonConverted)
            {
                print("AfterSerializingModel: " + json);
                string modifiedData = JsonEncryption.Encrypt(json);
                File.WriteAllText(FilePath, modifiedData);
            }
        }
    }
}
