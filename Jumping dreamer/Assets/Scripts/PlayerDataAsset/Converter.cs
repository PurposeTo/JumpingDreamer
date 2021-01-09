using Newtonsoft.Json;
using System;
using UnityEngine;

public static class Converter
{
    private static readonly JsonSerializerSettings serializerSettings;


    static Converter()
    {
        serializerSettings = new JsonSerializerSettings();
        serializerSettings.Converters.Add(new SafeIntConverter());
    }


    public static string ToJson(Data data, out bool isSuccess, out Exception exception)
    {
        try
        {
            string json = JsonConvert.SerializeObject(data, serializerSettings);
            isSuccess = true;
            exception = null;

            return json;
        }
        catch (Exception catchedException)
        {
            Debug.LogError($"Неуспешная попытка сериализации: {catchedException.Message}");

            isSuccess = false;
            exception = catchedException;

            return null;
        }
    }


    public static Data ToObject(string json, out bool isSuccess, out Exception exception)
    {
        try
        {
            Data data = JsonConvert.DeserializeObject<Data>(json, serializerSettings);
            isSuccess = true;
            exception = null;

            return data;
        }
        catch (Exception catchedException)
        {
            Debug.LogError($"Неуспешная попытка десериализации: {catchedException.Message}");

            isSuccess = false;
            exception = catchedException;

            return null;
        }
    }
}
