using System;
using Desdiene.GameDataAsset.Data;
using Desdiene.JsonConvertorWrapper;
using Desdiene.SuperMonoBehaviourAsset;
using Desdiene.Tools;
using UnityEngine;

namespace Desdiene.GameDataAsset.DataLoader.Storage
{
    public abstract class DataStorage<T> : ReaderWriter<T> where T : GameData, new()
    {
        public string Name { get; } // Имя конкретного хранилища

        protected string FileName { get; }
        protected string FileExtension { get; }
        protected string FileNameWithExtension => FileName + FileExtension;

        private readonly Validator validator = new Validator();
        private readonly IJsonConvertor<T> jsonConvertor;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="superMonoBehaviour"></param>
        /// <param name="storageName">Имя хранилища</param>
        /// <param name="fileName">Имя сохраняемого файла</param>
        /// <param name="fileName">расширение сохраняемого файла</param>
        /// <param name="serializerSettings">Настройки (де)сериализации данных</param>
        public DataStorage(SuperMonoBehaviour superMonoBehaviour,
            string storageName,
            string fileName,
            string fileExtension,
            IJsonConvertor<T> jsonConvertor)
            : base(superMonoBehaviour)
        {
            if (superMonoBehaviour is null) throw new ArgumentNullException(nameof(superMonoBehaviour));

            if (string.IsNullOrEmpty(storageName))
            {
                throw new ArgumentException($"{nameof(storageName)} не может быть пустым или иметь значение null", 
                    nameof(storageName));
            }

            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException($"{nameof(fileName)} не может быть пустым или иметь значение null",
                    nameof(fileName));
            }

            if (string.IsNullOrEmpty(fileExtension))
            {
                throw new ArgumentException($"{nameof(fileExtension)} не может быть пустым или иметь значение null",
                    nameof(fileExtension));
            }

            Name = storageName;
            FileName = FileName;
            FileExtension = fileExtension;
            this.jsonConvertor = jsonConvertor ?? throw new ArgumentNullException(nameof(jsonConvertor));
        }

        /// <summary>
        /// Возвращает коллбеком данные:
        /// 1. null, если данных нет в хранилище
        /// 2. объект данных, если они есть в хранилище.
        /// Не вызовется, если произошли проблемы при чтении.
        /// </summary>
        /// <param name="dataCallback"></param>
        public override void Read(Action<T> dataCallback)
        {
            Read(jsonData =>
            {
                if (string.IsNullOrEmpty(jsonData)) dataCallback?.Invoke(null);
                else
                {
                    T data = DeserializeData(jsonData);
                    data = TryToRepairNullFields(data);
                    dataCallback?.Invoke(data);
                }
            });
        }

        public override void Write(T data)
        {
            string jsonData = SerializeData(data);
            if (validator.HasJsonNullValues(jsonData)) return;
            else
            {
                Write(jsonData);
            }
        }

        protected abstract void Read(Action<string> jsonDataCallback);

        protected abstract void Write(string jsonData);

        /// <summary>
        /// Установить значения полям == null.
        /// Данная реализация создает экземпляр класса T.
        /// </summary>
        /// <param name="data">Объект, содержащий null поля</param>
        /// <returns>Объект, НЕ содержащий null поля</returns>
        protected virtual T RepairNullFields(T data)
        {
            //todo установить в null поля значения по умолчанию...
            return new T();
        }

        /// <summary>
        /// Починить json, если возникла ошибка десериализации.
        /// Ошибки возможны при плохой обратной совместимости объектов данных после их изменения в новых версиях.
        /// Текущая реализация возвращает пустой json объект.
        /// </summary>
        /// <param name="jsonData">json, который необходимо починить</param>
        /// <returns>корректный json</returns>
        protected virtual string RepairJson(string jsonData) => "{}";

        private T TryToRepairNullFields(T data)
        {
            if (new Validator().HasJsonNullValues(SerializeData(data)))
            {
                return RepairNullFields(data);
            }
            else return data;
        }

        private string SerializeData(T data)
        {
            return jsonConvertor.SerializeObject(data);
        }

        private T DeserializeData(string jsonData)
        {
            try
            {
                return jsonConvertor.DeserializeObject(jsonData);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
                return RepairJsonAndDeserialize(jsonData);
            }
        }

        private T RepairJsonAndDeserialize(string jsonData)
        {
            Debug.Log($"Start repairing json data:\n{jsonData}");

            string repairedJson = RepairJson(jsonData);

            try
            {
                return jsonConvertor.DeserializeObject(repairedJson);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
                return null;
            }
        }
    }
}
