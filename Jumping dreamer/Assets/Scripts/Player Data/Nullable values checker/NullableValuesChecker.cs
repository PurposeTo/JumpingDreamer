using System;
using System.Reflection;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
public class NullableValuesChecker
{
    //Not working...
    public void HaveNullableValues(PlayerDataModel model)
    {
        string json = JsonConverterWrapper.SerializeObject(model, null);

        JObject jObject = new JObject(json);

        Debug.LogWarning(jObject);
    }

}
