using System;
using System.Collections;
using System.IO;
using Desdiene.Coroutine.CoroutineExecutor;
using Desdiene.Super_monoBehaviour;
using UnityEngine;

public class LocalDataStorage : DataStorage
{
    private readonly string filePath;

    public LocalDataStorage(SuperMonoBehaviour superMonoBehaviour) : base(superMonoBehaviour)
    {
        filePath = DataLoaderHelper.GetFilePath(DataModel.FileNameWithExtension);
        loadDataInfo = superMonoBehaviour.CreateCoroutineContainer();
    }


    private ICoroutineContainer loadDataInfo;


    private protected override void ReadFromStorage(Action<PlayerGameData> dataCallback)
    {
        superMonoBehaviour.ExecuteCoroutineContinuously(ref loadDataInfo, LoadAndDecryptData(json =>
        {
            if (!new Validator().HasJsonNullValues(json))
            {
                base.data = DataConverter.ToObject(json, out bool isSuccess, out Exception exception);

                if (!isSuccess)
                {
                    Debug.LogError($"Возникла ошибка при чтении данных из файла: {exception}");

                    return;
                }
            }
            else return;

            dataCallback?.Invoke(base.data);
        }));
    }


    private protected override void WriteToStorage(PlayerGameData data)
    {
        SaveAndEncryptData(data);
    }


    private IEnumerator LoadAndDecryptData(Action<string> jsonAction)
    {
        Debug.Log($"Путь к файлу данных: {filePath}");

        yield return new DeviceDataLoader(filePath).LoadDataEnumerator(receivedData =>
        {
            if (receivedData != null) jsonAction?.Invoke(JsonEncryption.Decrypt(receivedData));
        });

    }


    private void SaveAndEncryptData(PlayerGameData data)
    {
        if (data == null) throw new ArgumentNullException(nameof(data));
        if (filePath == null) throw new ArgumentNullException(nameof(filePath));

        // TODO: А если у пользователя недостаточно памяти, чтобы создать файл?

        string json = DataConverter.ToJson(data, out bool isSerializationSuccess, out _);

        if (isSerializationSuccess)
        {
            Debug.Log("Сериализованные данные: " + json);

            string modifiedData = JsonEncryption.Encrypt(json);
            File.WriteAllText(filePath, modifiedData);
        }
        else PopUpWindowGenerator.Instance.CreateDialogWindow("Ошибка записи данных игровой статистики! Пожалуйста, обратитесь в службу поддержки.");
    }
}
