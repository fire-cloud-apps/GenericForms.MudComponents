using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace Blazor.Shared.FormGenerator.Conversions;

/// <summary>
/// Custom DateTime type to "dd/MM/yyyy HH:mm:ss"
/// </summary>
public class CustomDateTimeConverter : JsonConverter<DateTime>
{
    private readonly string _dateTimeFormat = "dd/MM/yyyy HH:mm:ss"; // Your JSON date format

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? dateString = reader.GetString();
        if (!string.IsNullOrEmpty(dateString) && DateTime.TryParseExact(dateString, _dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
        {
            return dateTime;
        }
        else
        {
            Console.WriteLine($"Error parsing date: '{dateString}' using format '{_dateTimeFormat}'");
            return DateTime.MinValue; // Or throw an exception
        }
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        Console.WriteLine($"Custom DateTime Write Invoked");
        writer.WriteStringValue(value.ToString(_dateTimeFormat, CultureInfo.InvariantCulture));
    }
}

