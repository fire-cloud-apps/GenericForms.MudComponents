using System.Text.Json;
using System.Text.Json.Serialization;
using MudBlazor;

namespace Blazor.Shared.FormGenerator.Conversions;


public class ColorTypeConverter : JsonConverter<Color>
{
    public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Enum.Parse<Color>(reader.GetString(), true);
    }

    public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
