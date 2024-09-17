using System.Text.Json;
using System.Text.Json.Serialization;
using MudBlazor;

namespace Blazor.Shared.FormGenerator.Conversions;

public class SelectionModeTypeConverter: JsonConverter<SelectionMode>
{
    public override SelectionMode Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Enum.Parse<SelectionMode>(reader.GetString(), true);
    }

    public override void Write(Utf8JsonWriter writer, SelectionMode value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}