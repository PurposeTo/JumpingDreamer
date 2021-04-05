using System;
using System.Collections;
using System.IO;
using Desdiene.Coroutine.CoroutineExecutor;
using Desdiene.GameDataAsset.Data;
using Desdiene.GameDataAsset.DataLoader.Storage;
using Desdiene.JsonConvertorWrapper;
using Desdiene.SuperMonoBehaviourAsset;
using Desdiene.Tools;
using UnityEngine;

namespace Desdiene.GameDataAsset.ConcreteStorages
{
    public class LocalStorage<T> : DataStorage<T>
         where T : GameData, new()
    {

        private readonly string filePath;

        private readonly ICoroutineContainer loadDataInfo;

        public LocalStorage(SuperMonoBehaviour superMonoBehaviour, 
            string fileName, 
            string fileExtension,
            IJsonConvertor<T> jsonConvertor)
            : base(superMonoBehaviour, 
                  "Локальное хранилище", 
                  fileName,
                  fileExtension,
                  jsonConvertor)
        {
            filePath = FilePathGetter.GetFilePath(FileNameWithExtension);
            loadDataInfo = superMonoBehaviour.CreateCoroutineContainer();
        }


        protected override void Read(Action<string> jsonDataCallback)
        {
            superMonoBehaviour.ExecuteCoroutineContinuously(loadDataInfo, LoadAndDecryptData(json => jsonDataCallback?.Invoke(json)));
        }

        protected override void Write(string jsonData)
        {
            SaveAndEncryptData(jsonData);
        }

        private IEnumerator LoadAndDecryptData(Action<string> jsonDataCallback)
        {
            Debug.Log($"Путь к файлу данных: {filePath}");

            yield return new DeviceDataLoader(filePath).LoadDataEnumerator(receivedData =>
            {
                jsonDataCallback?.Invoke(JsonEncryption.Decrypt(receivedData));
            });

        }

        private void SaveAndEncryptData(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData))
            {
                throw new ArgumentException($"{nameof(jsonData)} не может быть пустым или иметь значение null", nameof(jsonData));
            }

            if (filePath == null) throw new ArgumentNullException(nameof(filePath));

            // TODO: А если у пользователя недостаточно памяти, чтобы создать файл?

            string modifiedData = JsonEncryption.Encrypt(jsonData);
            File.WriteAllText(filePath, modifiedData);
        }
    }
}
