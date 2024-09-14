using System.Text.Json;
using System.Text.Json.Serialization;
using MudBlazor;

namespace Blazor.Shared.FormGenerator.Conversions;

public class AdornmentTypeConverter : JsonConverter<Adornment>
{
    public override Adornment Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Enum.Parse<Adornment>(reader.GetString(), true);
    }

    public override void Write(Utf8JsonWriter writer, Adornment value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}