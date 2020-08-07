using System;
using UnityEngine;
using System.IO;
using System.Text.Json;

public class PlayerStatsDataStorageSafe : SingletonMonoBehaviour<PlayerStatsDataStorageSafe>
{
    public PlayerStatsDataModel PlayerStatsData { get; private set; }

    public string FilePath { get; private set; }
    private readonly string fileName = "Stats.json"; // Имя файла с данными (как часть всего пути)
    JsonSerializerOptions serializeOptions; // Параметры сериализации

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
        FilePath = DataLoaderHelper.GetFilePath(fileName);
        Debug.Log($"File path: {FilePath}");

        // Инициализация параметров сериализации
        serializeOptions = new JsonSerializerOptions();
        serializeOptions.Converters.Add(new SafeIntConverter());
        serializeOptions.WriteIndented = true;

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

            // Если не было внешнего вмешательства в файлы с данными
            if (dataAsJSON != null)
            {
                try
                {
                    PlayerStatsData = JsonSerializer.Deserialize<PlayerStatsDataModel>(dataAsJSON, serializeOptions);
                }
                catch (Exception exception)
                {
                    isJsonConverted = false;

                    Debug.LogError($"Unsuccessful attempt of deserialization: {exception.Message}");
                    ErrorWindowGenerator.Instance.CreateErrorWindow($"{exception.Message}\nОшибка загрузки данных игровой статистики!\nЗапись новых данных заблокирована!");
                }
            }
            else
            {
                isJsonConverted = false;

                Debug.LogError($"Data reading from \"{fileName}\" ERROR!\nData was edited from outside.");
                ErrorWindowGenerator.Instance.CreateErrorWindow("Ошибка загрузки данных игровой статистики!\nЗапись новых данных заблокирована!");
            }

            if (isJsonConverted)
            {
                IsDataFileLoaded = true;
                Debug.Log($"Data from \"{fileName}\" was loaded successfully.");
            }
            else
            {
                PlayerStatsData = PlayerStatsDataModel.CreateModelWithDefaultValues();
                IsDataFileLoaded = false;
            }
        }
        else
        {
            // Установить значения по дефолту
            Debug.Log($"File path \"{FilePath}\" didn't found. Creating empty object...");

            PlayerStatsData = PlayerStatsDataModel.CreateModelWithDefaultValues();
            IsDataFileLoaded = true;
        }
    }


    public void DeletePlayerStatsData()
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
            print($"PlayerStatsData: {PlayerStatsData}");
            print($"PlayerStatsData.MaxEarnedScore: {PlayerStatsData.MaxEarnedScore}");
            print($"TypeOfConverter: {typeof(SafeIntConverter)}");

            string json = "";
            bool isJsonConverted = true;
            try
            {
                json = JsonSerializer.Serialize<PlayerStatsDataModel>(PlayerStatsData, serializeOptions);
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
