using UnityEngine;
using System.IO;

public class PlayerDataLocalStorageSafe
{
    public PlayerModelData Data { get; private set; } = null;
    public string FilePath { get; private set; }


    public void LoadDataFromFileAndDecrypt()
    {
        FilePath = DataLoaderHelper.GetFilePath(PlayerModel.FileNameWithExtension);
        Debug.Log($"File path: {FilePath}");

        Data = GetDecryptedData();
    }


    public void SaveDataToFileAndEncrypt(PlayerModelData modelData)
    {
        if (modelData is null) throw new System.ArgumentNullException(nameof(modelData));

        // TODO: А если у пользователя недостаточно памяти, чтобы создать файл?

        bool isSerializingSuccess = false;
        string json = JsonConverterWrapper.SerializeObject(modelData, (success, exception) => isSerializingSuccess = success);

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


    private PlayerModelData GetDecryptedData()
    {
        // Проверка на существование файла
        if (File.Exists(FilePath))
        {
            Debug.Log($"File on path \"{FilePath}\" was found.");

            string dataAsJSON = JsonEncryption.Decrypt(FilePath);
            #region без шифрования
            //string dataAsJSON = File.ReadAllText(FilePath); // без расшифрования
            #endregion

            return ValidateData(dataAsJSON);
        }
        else
        {
            // Установить значения по дефолту
            Debug.Log($"File path \"{FilePath}\" didn't found.");

            return null;
        }
    }


    private PlayerModelData ValidateData(string dataAsJSON)
    {
        PlayerModelData modelData = new PlayerModelData();

        bool IsJsonConverted()
        {
            bool isDeserializationSuccess = false;
            modelData = JsonConverterWrapper.DeserializeObject(dataAsJSON, (success, exception) => isDeserializationSuccess = success);

            return isDeserializationSuccess;
        }

        if (dataAsJSON == null || !IsJsonConverted() || new Validator().HasNullValues())
        {
            Debug.LogError($"Data reading from \"{PlayerModel.FileNameWithExtension}\" ERROR!\nData was edited from outside.");
            PopUpWindowGenerator.Instance.CreateDialogWindow("Ошибка загрузки данных игровой статистики!");

            return null;
        }
        else
        {
            Debug.Log($"Data from \"{PlayerModel.FileNameWithExtension}\" was loaded successfully.");

            return modelData;
        }
    }
}
