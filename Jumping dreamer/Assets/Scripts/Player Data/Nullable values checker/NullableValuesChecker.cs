using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;

public class NullableValuesChecker
{
    public bool HaveNullableValues(object model)
    {

        return GetAllPropertieInfosDeep(model, 5).Any(propertyInfo => propertyInfo.GetValue(model) is null);
    }


    public bool IsAnyStringNullOrEmpty(object model)
    {
        return model.GetType().GetProperties()
            .Where(pi => pi.PropertyType == typeof(string))
            .Select(pi => (string)pi.GetValue(model))
            .Any(value => string.IsNullOrEmpty(value));
    }


    private PropertyInfo[] GetAllPropertieInfosDeep(object model, int maxLvl, int lvlCounter = 0)
    {
        Type theType = model.GetType();
        List<PropertyInfo> propertyInfos = new List<PropertyInfo>();
        propertyInfos.AddRange(theType.GetProperties());

        foreach (PropertyInfo propertyInfo in propertyInfos)
        {
            if (propertyInfo.CanRead && propertyInfo.GetIndexParameters().Length == 0)
            {
                object child = propertyInfo.GetValue(model);

                if (lvlCounter < maxLvl)
                {
                    propertyInfos.AddRange(GetAllPropertieInfosDeep(child, maxLvl, lvlCounter + 1));
                }
            }
        }

        return propertyInfos.ToArray();
    }

}
