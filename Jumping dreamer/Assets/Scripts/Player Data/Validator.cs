using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

public class Validator
{
    public bool HasModelDataNullValues(PlayerModelData modelData)
    {
        string json = JsonConverterWrapper.SerializeObject(modelData, out bool isSuccess, out Exception exection);
        if (!isSuccess) return true;
        else return HasJsonNullValues(json);
    }


    public bool HasJsonNullValues(string stringAsJson)
    {
        JObject jObject = JObject.Parse(stringAsJson);

        Debug.Log($"Found JObject Start validating...\n{jObject}");

        foreach (KeyValuePair<string, JToken> item in jObject)
        {
            //Debug.LogWarning($"Checking KeyValuePair. Key = {item.Key}, Value = {item.Value}");

            switch (item.Value.Type)
            {
                case JTokenType.Object:
                    // Если true, то сразу вернуть значение
                    if (HasJsonNullValues(item.Value.ToString())) return true;
                    break;
                case JTokenType.Array:
                    // todo Преобразовать в массив?..
                    break;
                case JTokenType.Null:
                    Debug.LogWarning($"{item.Key} has null value!");
                    return true;
            }
        }

        return false;
    }
}
