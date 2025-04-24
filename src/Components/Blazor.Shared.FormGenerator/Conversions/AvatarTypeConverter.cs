using System.Text.Json;
using System.Text.Json.Serialization;
using Blazor.Shared.FormGenerator.Models;
using MudBlazor;

namespace Blazor.Shared.FormGenerator.Conversions;


public class AvatarTypeConverter : JsonConverter<AvatarType>
{
    public override AvatarType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Enum.Parse<AvatarType>(reader.GetString(), true);
    }

    public override void Write(Utf8JsonWriter writer, AvatarType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
