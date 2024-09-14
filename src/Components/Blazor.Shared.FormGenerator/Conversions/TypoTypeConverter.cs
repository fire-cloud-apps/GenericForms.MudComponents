using System.Text.Json;
using System.Text.Json.Serialization;
using MudBlazor;

namespace Blazor.Shared.FormGenerator.Conversions;

public class TypoTypeConverter : JsonConverter<Typo>
{
    public override Typo Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Enum.Parse<Typo>(reader.GetString(), true);
    }

    public override void Write(Utf8JsonWriter writer, Typo value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}