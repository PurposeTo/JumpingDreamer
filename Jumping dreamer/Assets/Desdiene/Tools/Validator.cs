using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;

namespace Desdiene.Tools
{
    public class Validator
    {
        public bool HasJsonNullValues(string stringAsJson)
        {
            try
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
            catch (Exception exception)
            {
                Debug.LogError(exception);

                return false;
            }
        }
    }
}
