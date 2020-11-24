using UnityEngine;
using System.IO;

public class PlayerDataLocalStorageSafe
{
    public PlayerDataModel LocalPlayerDataModel { get; private set; } = null;
    public string FilePath { get; private set; }


    public void LoadPlayerDataFromFileAndDecrypt()
    {
        FilePath = DataLoaderHelper.GetFilePath(PlayerDataModel.FileNameWithExtension);
        Debug.Log($"File path: {FilePath}");

        LocalPlayerDataModel = GetDecryptedPlayerData();
    }


    public void SaveDataToFileAndEncrypt(PlayerDataModel dataModel)
    {
        if (dataModel is null) throw new System.ArgumentNullException(nameof(dataModel));

        // TODO: А если у пользователя недостаточно памяти, чтобы создать файл?

        bool isSerializingSuccess = false;
        string json = JsonConverterWrapper.SerializeObject(dataModel, (success, exception) => isSerializingSuccess = success);

        if (isSerializingSuccess)
        {
            Debug.Log("After serializing model: " + json);
            string modifiedData = JsonEncryption.Encrypt(json);
            File.WriteAllText(FilePath, modifiedData);
            #region без шифрования
            //File.WriteAllText(FilePath, json); // без шифрования
            #endregion
        }
        else PopUpWindowGenerator.Instance.CreateDialogWindow("Ошибка записи данных игровой статистики! Пожалуйста, обратитесь в службу поддержки.");
    }


    private PlayerDataModel GetDecryptedPlayerData()
    {
        // Проверка на существование файла
        if (File.Exists(FilePath))
        {
            Debug.Log($"File on path \"{FilePath}\" was found.");

            string dataAsJSON = JsonEncryption.Decrypt(FilePath);
            #region без шифрования
            //string dataAsJSON = File.ReadAllText(FilePath); // без расшифрования
            #endregion

            return ValidateModel(dataAsJSON);
        }
        else
        {
            // Установить значения по дефолту
            Debug.Log($"File path \"{FilePath}\" didn't found.");

            return null;
        }
    }


    private PlayerDataModel ValidateModel(string dataAsJSON)
    {
        PlayerDataModel playerDataModel = null;

        bool IsJsonConverted()
        {
            bool isDeserializationSuccess = false;
            playerDataModel = JsonConverterWrapper.DeserializeObject(dataAsJSON, (success, exception) => isDeserializationSuccess = success);

            return isDeserializationSuccess;
        }

        if (dataAsJSON == null || !IsJsonConverted() || playerDataModel.IsModelHasNullValues())
        {
            Debug.LogError($"Data reading from \"{PlayerDataModel.FileNameWithExtension}\" ERROR!\nData was edited from outside.");
            PopUpWindowGenerator.Instance.CreateDialogWindow("Ошибка загрузки данных игровой статистики!");

            return null;
        }
        else
        {
            Debug.Log($"Data from \"{PlayerDataModel.FileNameWithExtension}\" was loaded successfully.");

            return playerDataModel;
        }
    }
}
