using UnityEngine;
using System.IO;

public class PlayerDataLocalStorageSafe
{
    public string FilePath { get; private set; }

    public bool IsDataFileLoaded { get; private set; } = false;


    public PlayerDataModel LoadPlayerData()
    {
        FilePath = DataLoaderHelper.GetFilePath(PlayerDataModel.FileName);
        Debug.Log($"File path: {FilePath}");

        return GetPlayerData();
    }


    public void DeletePlayerData()
    {
        File.Delete(FilePath);
        File.Delete(FilePath + ".meta");
        File.Delete(JsonEncryption.FilePathWithHash);
        File.Delete(JsonEncryption.FilePathWithHash + ".meta");

        IsDataFileLoaded = true; // Снова можем записывать информацию в файл
    }


    public void WritePlayerDataToFile(PlayerDataModel playerDataModel)
    {
        if (IsDataFileLoaded)
        {
            // TODO: А если у пользователя недостаточно памяти, чтобы создать файл?

            string json = "";
            bool isJsonConverted = true;

            json = JsonConverterWrapper.SerializeObject(playerDataModel, (success, exception) =>
            {
                if (!success)
                {
                    isJsonConverted = false;

                    DialogWindowGenerator.Instance.CreateErrorWindow("Ошибка записи данных игровой статистики! Пожалуйста, обратитесь в службу поддержки.");
                }
            });

            if (isJsonConverted)
            {
                Debug.Log("AfterSerializingModel: " + json);
                string modifiedData = JsonEncryption.Encrypt(json);
                File.WriteAllText(FilePath, modifiedData);
            }
        }
    }


    private PlayerDataModel GetPlayerData()
    {
        // Проверка на существование файла
        if (File.Exists(FilePath))
        {
            Debug.Log($"File on path \"{FilePath}\" was found.");

            string dataAsJSON = JsonEncryption.Decrypt(FilePath);
            return ValidateModel(dataAsJSON);
        }
        else
        {
            // Установить значения по дефолту
            Debug.Log($"File path \"{FilePath}\" didn't found. Creating empty object...");

            IsDataFileLoaded = true;
            return PlayerDataModel.CreateModelWithDefaultValues();
        }
    }


    private PlayerDataModel ValidateModel(string dataAsJSON)
    {
        PlayerDataModel playerDataModel = null;

        bool IsJsonConverted()
        {
            bool returnedSuccess = false;

            playerDataModel = JsonConverterWrapper.DeserializeObject(dataAsJSON, (success, exception) =>
            {
                returnedSuccess = success;

                if (!success)
                {
                    DialogWindowGenerator.Instance.CreateErrorWindow($"{exception.Message}\nОшибка загрузки данных игровой статистики!\nЗапись новых данных заблокирована!");
                }
            });

            return returnedSuccess;
        }

        if (dataAsJSON == null || !IsJsonConverted() || playerDataModel.IsModelHasNullValues())
        {
            IsDataFileLoaded = false;

            Debug.LogError($"Data reading from \"{PlayerDataModel.FileName}\" ERROR!\nData was edited from outside.");
            DialogWindowGenerator.Instance.CreateErrorWindow("Ошибка загрузки данных игровой статистики!\nЗапись новых данных заблокирована!");

            return PlayerDataModel.CreateModelWithDefaultValues();
        }
        else
        {
            IsDataFileLoaded = true;
            Debug.Log($"Data from \"{PlayerDataModel.FileName}\" was loaded successfully.");

            return playerDataModel;
        }
    }
}
