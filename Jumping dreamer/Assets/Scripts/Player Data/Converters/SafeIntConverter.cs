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
            throw new JsonException($"Unexpected token or value when parsing SafeInt! " +
                $"JsonToken = {reader.TokenType}");
        }
    }


    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        if (value is SafeInt) // value is SafeInt? (nullable)
        {
            writer.WriteValue((SafeInt)value);
        }
        else
        {
            throw new JsonSerializationException("Expected SafeInt object value");
        }
    }
}
