using System;
using System.IO;
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

        protected readonly string filePath;
        protected readonly DeviceDataLoader deviceDataLoader;

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
            Debug.Log($"{Name}. Путь к файлу данных : {filePath}");
            deviceDataLoader = new DeviceDataLoader(superMonoBehaviour, filePath);
        }

        protected override void Read(Action<string> jsonDataCallback)
        {
            deviceDataLoader.LoadDataFromDevice(jsonDataCallback.Invoke);
        }

        protected override void Write(string jsonData)
        {
            if (string.IsNullOrEmpty(jsonData))
            {
                throw new ArgumentException($"{nameof(jsonData)} не может быть пустым или иметь значение null", nameof(jsonData));
            }

            // TODO: А если у пользователя недостаточно памяти, чтобы создать файл?

            File.WriteAllText(filePath, jsonData);
        }
    }
}
