using System;
using UnityEngine;
using System.IO;

public class PlayerDataStorageSafe : SingletonMonoBehaviour<PlayerDataStorageSafe>
{
    public PlayerDataModel PlayerDataModel { get; private set; }

    public string FilePath { get; private set; }
    private readonly string fileName = "GameData.json"; // Имя файла с данными (как часть всего пути)

    public bool IsDataFileLoaded { get; private set; } = false;

    public event EventHandler OnDeleteStats;
    public static bool IsPlayerDataAlreadyReset { get; private set; } = false;



    protected override void AwakeSingleton()
    {
        LoadPlayerData();
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


    private void LoadPlayerData()
    {
        FilePath = DataLoaderHelper.GetFilePath(fileName);
        Debug.Log($"File path: {FilePath}");

        GetPlayerData();
    }


    private void GetPlayerData()
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
                PlayerDataModel = JsonConverterWrapper.DeserializeObject(dataAsJSON, (success, exception) =>
                {
                    if (success)
                    {
                        print("#MaxCollectedStars: " + PlayerDataModel.PlayerStats.MaxCollectedStars);
                        print("#MaxEarnedScore: " + PlayerDataModel.PlayerStats.MaxEarnedScore);
                        print("#MaxLifeTime: " + PlayerDataModel.PlayerStats.MaxLifeTime);

                        // Структура json соответствует модели данных?
                        isJsonStructureIncorrect = PlayerDataModel.IsModelHasNullValues();
                    }
                    else
                    {
                        isJsonConverted = false;

                        ErrorWindowGenerator.Instance.CreateErrorWindow($"{exception.Message}\nОшибка загрузки данных игровой статистики!\nЗапись новых данных заблокирована!");
                    }
                });
            }
            
            if(dataAsJSON == null || isJsonStructureIncorrect || !isJsonConverted)
            {
                PlayerDataModel = PlayerDataModel.CreateModelWithDefaultValues();
                print("#MaxCollectedStars: " + PlayerDataModel.PlayerStats.MaxCollectedStars);
                print("#MaxEarnedScore: " + PlayerDataModel.PlayerStats.MaxEarnedScore);
                print("#MaxLifeTime: " + PlayerDataModel.PlayerStats.MaxLifeTime);
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

            PlayerDataModel = PlayerDataModel.CreateModelWithDefaultValues();
            print("#MaxCollectedStars: " + PlayerDataModel.PlayerStats.MaxCollectedStars);
            print("#MaxEarnedScore: " + PlayerDataModel.PlayerStats.MaxEarnedScore);
            print("#MaxLifeTime: " + PlayerDataModel.PlayerStats.MaxLifeTime);
            IsDataFileLoaded = true;
        }
    }


    public void DeletePlayerStatsData()
    {
        PlayerDataModel = PlayerDataModel.CreateModelWithDefaultValues();

        File.Delete(FilePath);
        File.Delete(FilePath + ".meta");
        File.Delete(JsonEncryption.FilePathWithHash);
        File.Delete(JsonEncryption.FilePathWithHash + ".meta");

        IsDataFileLoaded = true; // Снова можем записывать информацию в файл
        IsPlayerDataAlreadyReset = true;
        OnDeleteStats?.Invoke(this, null);

        // TODO: Загрузка новых данных в облако
    }


    private void WritePlayerDataToFile()
    {
        if (IsDataFileLoaded)
        {
            // А если у пользователя недостаточно памяти, чтобы создать файл?
            print($"#PlayerStatsData: {PlayerDataModel}");
            print($"#PlayerStatsData.MaxEarnedScore: {PlayerDataModel.PlayerStats.MaxEarnedScore}");

            string json = "";
            bool isJsonConverted = true;

            json = JsonConverterWrapper.SerializeObject(PlayerDataModel, (success, exception) =>
            {
                if (!success)
                {
                    isJsonConverted = false;

                    ErrorWindowGenerator.Instance.CreateErrorWindow("Ошибка записи данных игровой статистики! Пожалуйста, обратитесь в службу поддержки.");
                }
            });

            if (isJsonConverted)
            {
                print("AfterSerializingModel: " + json);
                string modifiedData = JsonEncryption.Encrypt(json);
                File.WriteAllText(FilePath, modifiedData);
            }
        }
    }
}
