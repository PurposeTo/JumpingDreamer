using System;
using System.Text.Json;
using System.Text.Json.Serialization;

public class SafeIntConverter : JsonConverter<SafeInt>
{
    public override SafeInt Read(
        ref Utf8JsonReader reader,
        Type typeToConvert,
        JsonSerializerOptions options) => new SafeInt(reader.GetInt32());


    public override void Write(
        Utf8JsonWriter writer,
        SafeInt value,
        JsonSerializerOptions options) => writer.WriteNumberValue(value);
}
