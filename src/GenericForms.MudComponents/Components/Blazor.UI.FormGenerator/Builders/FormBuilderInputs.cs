using System.Text.Json.Serialization;
using System.Text.Json;
using MudBlazor;

namespace Blazor.UI.FormGenerator.Builders;

/// <summary>
/// Form Builder Input as Json string
/// </summary>
public class FormBuilderInputs
{
    /// <summary>
    /// A Unique 'Id' to assign the forms.
    /// </summary>
    public string Id { get; set; } =  Guid.NewGuid().ToString();
    
    /// <summary>
    /// Consider this as Model Name
    /// </summary>
    public string FieldName { get; set; }
    /// <summary>
    /// The text field will apply formatting rules or input restrictions on-the-fly while the user is typing. eg."0000 0000 0000 0000", "+(91)-(00000)-(00000)". etc.
    /// </summary>
    public string PatternMask { get; set; }
    public string Label { get; set; }
    [JsonConverter(typeof(InputFieldTypeConverter))]
    public InputFieldType InputType { get; set; }
    public SelectionItems[] SelectItemList { get; set; }
    public bool IsReadOnly { get; set; }
    public string DefaultValue { get; set; }
    [JsonConverter(typeof(VariantTypeConverter))]
    public Variant Variant { get; set; }
    public GridPosition GridPosition { get; set; }
    public bool IsMultiple { get; set; } = false;
    public string FileAllowed { get; set; } = string.Empty;

    public FormValidation Validation { get; set; } = new FormValidation()
    {
        IsRequired = false,
        RequiredError = ""
    };

    #region Text Box Releated Settings
    public int Line { get; set; } = 3;
    #endregion

    #region Title Sepecific Settings
    [JsonConverter(typeof(TypoTypeConverter))]
    public Typo TextStyle { get; set; }

    /// <summary>
    /// justify-start | justify-center | justify-end | justify-space-between | justify-space-around
    /// <see cref="https://mudblazor.com/utilities/justify-content#applying-conditionally"/>
    /// </summary>
    public string TileAlign { get; set; } = "justify-center";

    #endregion

    #region Alert Message
    [JsonConverter(typeof(SeverityTypeConverter))]
    public Severity Severity { get; set; } = Severity.Normal;

    #endregion
}







public class InputFieldTypeConverter : JsonConverter<InputFieldType>
{
    public override InputFieldType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return Enum.Parse<InputFieldType>(reader.GetString(), true);
    }

    public override void Write(Utf8JsonWriter writer, InputFieldType value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}

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