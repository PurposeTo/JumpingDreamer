using System;
using Desdiene.GameDataAsset.Data;
using Desdiene.SuperMonoBehaviourAsset;
using Desdiene.Tools;
using Newtonsoft.Json;
using UnityEngine;

namespace Desdiene.GameDataAsset.DataLoader.Storage
{
    public abstract class DataStorage<T> : ReaderWriter<T> where T : GameData, new()
    {
        public string Name { get; } // Имя конкретного хранилища

        private readonly Validator validator = new Validator();
        private readonly JsonSerializerSettings serializerSettings;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="superMonoBehaviour"></param>
        /// <param name="name">Имя хранилища</param>
        /// <param name="serializerSettings">Настройки (де)сериализации данных</param>
        public DataStorage(SuperMonoBehaviour superMonoBehaviour, string name, JsonSerializerSettings serializerSettings)
            : base(superMonoBehaviour)
        {
            if (superMonoBehaviour is null) throw new ArgumentNullException(nameof(superMonoBehaviour));

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"{nameof(name)} не может быть пустым или иметь значение null", nameof(name));
            }

            Name = name;
            this.serializerSettings = serializerSettings ?? throw new ArgumentNullException(nameof(serializerSettings));
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

        protected abstract void Read(Action<string> dataCallback);

        protected abstract void Write(string jsonData);

        private T TryToRepairNullFields(T data)
        {
            if (new Validator().HasJsonNullValues(SerializeData(data)))
            {
                return RepairNullFields(data);
            }
            else return data;
        }

        private T RepairNullFields(T data)
        {
            //todo установить в null поля значения по умолчанию...
            return new T();
        }

        private string SerializeData(T data)
        {
            return JsonConvert.SerializeObject(data, serializerSettings);
        }

        private T DeserializeData(string jsonData)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(jsonData, serializerSettings);
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
                return JsonConvert.DeserializeObject<T>(repairedJson, serializerSettings);
            }
            catch (Exception exception)
            {
                Debug.LogError(exception.Message);
                return null;
            }
        }

        private string RepairJson(string jsonData)
        {
            /*
             * todo
             * Починить json.
             * Ошибки возможны при плохой обратной совместимости объектов данных после их изменения в новых версиях.
             */
            return jsonData;
        }
    }
}
