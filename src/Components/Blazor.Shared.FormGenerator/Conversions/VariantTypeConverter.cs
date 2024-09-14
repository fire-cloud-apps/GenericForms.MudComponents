using System.Text.Json;
using System.Text.Json.Serialization;
using MudBlazor;

namespace Blazor.Shared.FormGenerator.Conversions;

public class VariantTypeConverter : JsonConverter<Variant>
{
    public override Variant Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Enum.Parse<Variant>(reader.GetString(), true);
    }

    public override void Write(Utf8JsonWriter writer, Variant value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}