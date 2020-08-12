using System;
using Newtonsoft.Json;

public class SafeIntConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(SafeInt);
    }


    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Integer)
        {
            try
            {
                return new SafeInt(Convert.ToInt32(reader.Value));
            }
            catch (Exception exception)
            {
                throw new JsonException($"Error parsing SafeInt: {exception}");
            }
        }
        else
        {
            //return new SafeInt(default(int)); // Если внутри json лежит null, то эта ветвь кода сработает. Если значение стерто полностью, то конструктор просто положит null в поле, которое отсутствует, при этом считывание пройдет успешно.
            throw new JsonException($"Unexpected token or value when parsing SafeInt!");
        }
    }


    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value is SafeInt) // value is SafeInt?
        {
            writer.WriteValue((SafeInt)value);
        }
        else
        {
            throw new JsonSerializationException("Expected SafeInt object value");
        }
    }
}
