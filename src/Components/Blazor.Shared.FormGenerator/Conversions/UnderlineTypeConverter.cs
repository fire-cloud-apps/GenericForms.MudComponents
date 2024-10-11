using System.Text.Json;
using System.Text.Json.Serialization;
using MudBlazor;

namespace Blazor.Shared.FormGenerator.Conversions;


public class UnderlineTypeConverter : JsonConverter<Underline>
{
    public override Underline Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Enum.Parse<Underline>(reader.GetString(), true);
    }

    public override void Write(Utf8JsonWriter writer, Underline value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
