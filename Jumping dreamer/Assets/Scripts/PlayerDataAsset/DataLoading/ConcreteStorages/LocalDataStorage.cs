using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class LocalDataStorage : DataStorage
{
    private ICoroutineContainer loadDataInfo;


    public LocalDataStorage(SuperMonoBehaviour superMonoBehaviour) : base(superMonoBehaviour)
    {
        loadDataInfo = superMonoBehaviour.CreateCoroutineContainer();
    }


    private string filePath;


    public override void Read(Action<Data> callback)
    {
        superMonoBehaviour.ExecuteCoroutineContinuously(ref loadDataInfo, LoadAndDecryptData(json =>
        {
            if (!new Validator().HasJsonNullValues(json))
            {
                data = Converter.ToObject(json, out bool isSuccess, out Exception exception);

                if (!isSuccess)
                {
                    Debug.LogError($"Возникла ошибка при чтении данных из файла: {exception}");

                    return;
                }
            }
            else return;

            callback?.Invoke(data);
        }));
    }


    public override void Write(Data data)
    {
        SaveAndEncryptData(data);
    }


    private IEnumerator LoadAndDecryptData(Action<string> jsonAction)
    {
        filePath = DataLoaderHelper.GetFilePath(DataModel.FileNameWithExtension);
        Debug.Log($"Путь к файлу данных: {filePath}");

        string data = null;
        yield return new DeviceDataLoader(filePath).LoadDataEnumerator(receivedData => data = receivedData);

        if (data != null) jsonAction?.Invoke(JsonEncryption.Decrypt(data));
    }


    private void SaveAndEncryptData(Data data)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));

        // TODO: А если у пользователя недостаточно памяти, чтобы создать файл?

        string json = Converter.ToJson(data, out bool isSerializationSuccess, out _);

        if (isSerializationSuccess)
        {
            Debug.Log("Сериализованные данные: " + json);

            string modifiedData = JsonEncryption.Encrypt(json);
            File.WriteAllText(filePath, modifiedData);
        }
        else PopUpWindowGenerator.Instance.CreateDialogWindow("Ошибка записи данных игровой статистики! Пожалуйста, обратитесь в службу поддержки.");
    }
}
