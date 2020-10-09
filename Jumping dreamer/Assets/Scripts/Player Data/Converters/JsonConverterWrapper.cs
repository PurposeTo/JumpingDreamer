using Newtonsoft.Json;
using System;
using UnityEngine;

public static class JsonConverterWrapper
{
    private static JsonSerializerSettings serializerSettings;


    static JsonConverterWrapper()
    {
        serializerSettings = new JsonSerializerSettings();
        serializerSettings.Converters.Add(new SafeIntConverter());
    }


    public static string SerializeObject(PlayerDataModel playerDataModel, Action<bool, Exception> onSerialize)
    {
        try
        {
            string json = JsonConvert.SerializeObject(playerDataModel, serializerSettings);
            onSerialize?.Invoke(true, null);

            return json;
        }
        catch (Exception exception)
        {
            Debug.LogError($"Unsuccessful attempt of serialization: {exception.Message}");
            onSerialize?.Invoke(false, exception);

            return null;
        }
    }


    public static PlayerDataModel DeserializeObject(string json, Action<bool, Exception> onDeserialize)
    {
        try
        {
            PlayerDataModel playerDataModel = JsonConvert.DeserializeObject<PlayerDataModel>(json, serializerSettings);
            onDeserialize?.Invoke(true, null);
            Debug.Log("Успешная десериализация!");

            return playerDataModel;
        }
        catch (Exception exception)
        {
            Debug.LogError($"Unsuccessful attempt of deserialization: {exception.Message}");
            onDeserialize?.Invoke(false, exception);

            Debug.Log("Неуспешная десериализация!");
            return null;
        }

    }
}
