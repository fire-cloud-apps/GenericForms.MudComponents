using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blazor.Shared.FormGenerator.Conversions;

public class MarginTypeConverter : JsonConverter<Margin>
{
    public override Margin Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Enum.Parse<Margin>(reader.GetString(), true);
    }

    public override void Write(Utf8JsonWriter writer, Margin value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}


