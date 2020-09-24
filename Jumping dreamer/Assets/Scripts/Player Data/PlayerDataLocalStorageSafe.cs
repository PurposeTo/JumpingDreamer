using UnityEngine;
using System.IO;

public class PlayerDataLocalStorageSafe
{
    public string FilePath { get; private set; }

    // Public set, т.к. в соответствии с архитектурой приложения объект этого класса может находиться только в контроллере. При этом так как это часть контроллера, то он может напрямую изменять значения этого свойства.
    public bool IsDataFileLoaded { get; set; } = false;


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


    public void WritePlayerDataToFile(PlayerDataModel localPlayerDataModel)
    {
        if (localPlayerDataModel == null) throw new System.ArgumentNullException("Local model can't have null value!");

        if (IsDataFileLoaded)
        {
            // TODO: А если у пользователя недостаточно памяти, чтобы создать файл?

            bool isSerializingSuccess = false;
            string json = JsonConverterWrapper.SerializeObject(localPlayerDataModel, (success, exception) => isSerializingSuccess = success);

            if (isSerializingSuccess)
            {
                Debug.Log("AfterSerializingModel: " + json);
                //string modifiedData = JsonEncryption.Encrypt(json);
                //File.WriteAllText(FilePath, modifiedData);
                File.WriteAllText(FilePath, json);
            }
            else DialogWindowGenerator.Instance.CreateDialogWindow("Ошибка записи данных игровой статистики! Пожалуйста, обратитесь в службу поддержки.");
        }
    }


    private PlayerDataModel GetPlayerData()
    {
        // Проверка на существование файла
        if (File.Exists(FilePath))
        {
            Debug.Log($"File on path \"{FilePath}\" was found.");

            //string dataAsJSON = JsonEncryption.Decrypt(FilePath);
            string dataAsJSON = File.ReadAllText(FilePath);
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
                    DialogWindowGenerator.Instance.CreateDialogWindow($"{exception.Message}\nОшибка загрузки данных игровой статистики!\nЗапись новых данных заблокирована!");
                }
            });

            return returnedSuccess;
        }

        if (dataAsJSON == null || !IsJsonConverted() || playerDataModel.IsModelHasNullValues())
        {
            IsDataFileLoaded = false;

            Debug.LogError($"Data reading from \"{PlayerDataModel.FileName}\" ERROR!\nData was edited from outside.");
            DialogWindowGenerator.Instance.CreateDialogWindow("Ошибка загрузки данных игровой статистики!\nЗапись новых данных заблокирована!");

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
