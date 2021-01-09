using UnityEngine;
using System.IO;

public class PlayerDataLocalStorageSafe
{
    public PlayerModelData Data { get; private set; } = null;
    public string FilePath { get; private set; }


    public void LoadValidatedSourceDataFromFile()
    {
        FilePath = DataLoaderHelper.GetFilePath(PlayerModel.FileNameWithExtension);
        Debug.Log($"File path: {FilePath}");

        string fileData = Load();

        if (fileData != null)
        {
            string dataAsJSON = JsonEncryption.Decrypt(fileData);
            Data = GetValidatedData(dataAsJSON);
        }
        else Data = null;
    }


    public void SaveDataToFileAndEncrypt(PlayerModelData modelData)
    {
        if (modelData == null) throw new System.ArgumentNullException(nameof(modelData));

        // TODO: А если у пользователя недостаточно памяти, чтобы создать файл?

        string json = JsonConverterWrapper.SerializeObject(modelData, 
            out bool isSerializationSuccess, out _);

        if (isSerializationSuccess)
        {
            Debug.Log("After serializing model: " + json);
            string modifiedData = JsonEncryption.Encrypt(json);
            File.WriteAllText(FilePath, modifiedData);
        }
        else PopUpWindowGenerator.Instance.CreateDialogWindow("Ошибка записи данных игровой статистики! Пожалуйста, обратитесь в службу поддержки.");
    }


    private string Load()
    {
        if (File.Exists(FilePath))
        {
            Debug.Log($"File on path \"{FilePath}\" was found.");
            return File.ReadAllText(FilePath);
        }
        else
        {
            Debug.Log($"File path \"{FilePath}\" didn't found.");
            return null;
        }
    }


    private PlayerModelData GetValidatedData(string dataAsJSON)
    {
        PlayerModelData modelData;

        bool IsJsonConverted()
        {
            modelData = JsonConverterWrapper.DeserializeObject(dataAsJSON, 
                out bool isDeserializationSuccess, out _);

            return isDeserializationSuccess;
        }

        if (dataAsJSON == null || !IsJsonConverted() /*|| new Validator().HasModelDataNullValues(modelData)*/)
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
