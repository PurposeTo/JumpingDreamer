using Newtonsoft.Json;
using UnityEngine;

namespace Desdiene.JsonConvertorWrapper
{
    /// <summary>
    /// Данный класс используется для хранения serializerSettings.
    /// Имеет методы (де)сериализации json-а.
    /// Обращаться через интерфейс.
    /// </summary>
    /// <typeparam name="T">Тип (де)сериализуемого объекта</typeparam>
    public class NewtonsoftJsonConvertor<T> : IJsonConvertor<T>
    {
        private readonly JsonSerializerSettings serializerSettings;

        public NewtonsoftJsonConvertor() : this(new JsonSerializerSettings()) { }

        public NewtonsoftJsonConvertor(JsonSerializerSettings serializerSettings)
        {
            this.serializerSettings = serializerSettings;
        }


        T IJsonConvertor<T>.DeserializeObject(string jsonData)
        {
            return JsonConvert.DeserializeObject<T>(jsonData, serializerSettings);
        }


        string IJsonConvertor<T>.SerializeObject(T data)
        {
            string jsonData = JsonConvert.SerializeObject(data, serializerSettings);
            Debug.Log("Сериализованные данные: " + jsonData);
            return jsonData;
        }

    }
}
