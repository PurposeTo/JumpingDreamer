using System;
using Newtonsoft.Json.Linq;

public class Validator
{
    public bool HasModelDataNullValues(PlayerModelData modelData)
    {
        string json = JsonConverterWrapper.SerializeObject(modelData, out bool isSuccess, out Exception exection);
        if (!isSuccess) return true;
        else return HasNullValues(json);
    }


    public bool HasNullValues(string stringAsJson)
    {
        // Debug!
        stringAsJson = JsonConverterWrapper.SerializeObject(new PlayerModelData(), out bool isSuccess, out Exception exection);

        var jObject = JObject.Parse(stringAsJson);

        UnityEngine.Debug.LogWarning(jObject);

        // Debug!
        return false;
    }
}
