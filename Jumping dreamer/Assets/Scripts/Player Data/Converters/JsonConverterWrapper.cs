using Newtonsoft.Json;
using System;
using UnityEngine;

public static class JsonConverterWrapper
{
    private static readonly JsonSerializerSettings serializerSettings;


    static JsonConverterWrapper()
    {
        serializerSettings = new JsonSerializerSettings();
        serializerSettings.Converters.Add(new SafeIntConverter());
    }


    public static string SerializeObject(PlayerModelData playerModelData, out bool isSuccess, out Exception exception)
    {
        try
        {
            string json = JsonConvert.SerializeObject(playerModelData, serializerSettings);
            isSuccess = true;
            exception = null;

            return json;
        }
        catch (Exception catchedException)
        {
            Debug.LogError($"Unsuccessful attempt of serialization: {catchedException.Message}");
            isSuccess = false;
            exception = catchedException;

            return null;
        }
    }


    public static PlayerModelData DeserializeObject(string json, out bool isSuccess, out Exception exception)
    {
        try
        {
            PlayerModelData playerModelData = JsonConvert.DeserializeObject<PlayerModelData>(json, serializerSettings);
            Debug.Log("Успешная десериализация!");
            isSuccess = true;
            exception = null;

            return playerModelData;
        }
        catch (Exception catchedException)
        {
            Debug.LogError($"Unsuccessful attempt of deserialization: {catchedException.Message}");
            isSuccess = false;
            exception = catchedException;
            Debug.Log("Неуспешная десериализация!");

            return null;
        }

    }
}
