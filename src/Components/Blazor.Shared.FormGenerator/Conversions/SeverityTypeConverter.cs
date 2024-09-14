using System.Text.Json;
using System.Text.Json.Serialization;
using MudBlazor;

namespace Blazor.Shared.FormGenerator.Conversions;

public class SeverityTypeConverter : JsonConverter<Severity>
{
    public override Severity Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Enum.Parse<Severity>(reader.GetString(), true);
    }

    public override void Write(Utf8JsonWriter writer, Severity value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}