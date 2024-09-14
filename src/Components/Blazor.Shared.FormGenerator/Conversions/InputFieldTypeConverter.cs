using System.Text.Json;
using System.Text.Json.Serialization;

namespace Blazor.Shared.FormGenerator.Conversions;

public class InputFieldTypeConverter : JsonConverter<InputFieldType>
{
    public override InputFieldType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Enum.Parse<InputFieldType>(reader.GetString(), true);
    }

    public override void Write(Utf8JsonWriter writer, InputFieldType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}