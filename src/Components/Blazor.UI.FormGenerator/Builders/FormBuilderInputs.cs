using System.Diagnostics.SymbolStore;
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
    
    /// <summary>
    /// It is the name of the field, which appears in top of the text box/select box/date/ etc.
    /// </summary>
    public string Label { get; set; }
    
    /// <summary>
    /// This determines, what control we need, is it a text input, number or date etc.
    /// </summary>
    [JsonConverter(typeof(InputFieldTypeConverter))]
    public InputFieldType InputType { get; set; }
    
    /// <summary>
    /// SelectItemList, is applicable for the 'MudSelect' and 'MudRadio' Currently in futuer if required to have multiple selection option on a control, this will be used.
    /// </summary>
    public SelectionItems[] SelectItemList { get; set; }

    /// <summary>
    /// This applies 'ReadOnly' ability to a control. Default is 'false'.
    /// </summary>
    public bool IsReadOnly { get; set; } = false;
    /// <summary>
    /// This applies 'Default' value to a text box or applicable controls.
    /// </summary>
    public string DefaultValue { get; set; }
    
    /// <summary>
    /// Variant Types 'Filled/Outlined/Text' styles can be applied.
    /// </summary>
    [JsonConverter(typeof(VariantTypeConverter))]
    public Variant Variant { get; set; }
    
    /// <summary>
    /// Indicates how to position a control, currenlty this supports only Grid based positioning.
    /// </summary>
    public GridPosition GridPosition { get; set; }
    
    /// <summary>
    /// This indicates that, what extension is allowed as a part of file upload.
    /// Eg. ".png, .jpg" or ".pdf, .doc" etc.
    /// </summary>
    public string FileAllowed { get; set; } = string.Empty;

    /// <summary>
    /// This indicates the extended 'Validation' function for a Form.
    /// </summary>
    public FormValidation Validation { get; set; } = new FormValidation()
    {
        IsRequired = false,
        RequiredError = ""
    };

    #region Text Box Releated Settings
    /// <summary>
    /// This is applied on the Text Input, where it requires, multiple line of items to provide input. eg. 'Description', 'Message' etc.
    /// </summary>
    public int Line { get; set; } = 3;
    #endregion

    #region Title Sepecific Settings
    /// <summary>
    /// Indicates what style to be applied to a Text Title, eg. h1, h2, h3 etc.
    /// </summary>
    [JsonConverter(typeof(TypoTypeConverter))]
    public Typo TextStyle { get; set; }

    /// <summary>
    /// justify-start | justify-center | justify-end | justify-space-between | justify-space-around
    /// <see cref="https://mudblazor.com/utilities/justify-content#applying-conditionally"/>
    /// </summary>
    public string TileAlign { get; set; } = "justify-center";

    #endregion

    #region Image
    /// <summary>
    /// This applies image to a image control. eg. '/img/student.png'.
    /// </summary>
    public string ImageSrc { get; set; } = string.Empty;
    /// <summary>
    /// This indicates the responsiveness for an image
    /// </summary>
    public bool Fluid { get; set; } = true;
    /// <summary>
    /// Alternative image text
    /// </summary>
    public string Alt { get; set; } = string.Empty;
    /// <summary>
    /// Custom class which currently applies in Image control
    /// </summary>
    public string Class { get; set; } = "rounded-lg";
    /// <summary>
    /// Applies to image, sets With of the image
    /// </summary>
    public int Width { get; set; } = 180;
    /// <summary>
    /// Applies to image, sets Height of the image
    /// </summary>
    public int Height { get; set; } = 180;
    #endregion

    #region Alert Message
    /// <summary>
    /// If a alert message with the Severity type indicator
    /// </summary>
    [JsonConverter(typeof(SeverityTypeConverter))]
    public Severity Severity { get; set; } = Severity.Normal;

    #endregion
}

public class IconTypeConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        //Console.WriteLine($"IconTypeConverter Before : {value}");
        var iconIdentifier = value.Split('.');

        Type iconType = null;
        switch (iconIdentifier[0])//Custom or Material
        {
            case "Material":
                default:
                switch (iconIdentifier[1])
                {
                    case "Filled":
                        default:
                        iconType = typeof(MudBlazor.Icons.Material.Filled);
                        break;
                    case "Outlined":
                        iconType = typeof(MudBlazor.Icons.Material.Outlined);
                        break;
                    case "Rounded":
                        iconType = typeof(MudBlazor.Icons.Material.Rounded);
                        break;
                    case "Sharp":
                        iconType = typeof(MudBlazor.Icons.Material.Sharp);
                        break;
                    case "TwoTone":
                        iconType = typeof(MudBlazor.Icons.Material.TwoTone);
                        break;
                }
                break;
            case "Custom":
                switch (iconIdentifier[1])
                {
                    case "Brands":
                    default:
                        iconType = typeof(Icons.Custom.Brands);
                        break;
                    case "Uncategorized":
                        iconType = typeof(Icons.Custom.Uncategorized);
                        break;
                    case "FileFormats":
                        iconType = typeof(Icons.Custom.FileFormats);
                        break;
                }
                break;
            case "UserDefined":
                break;
        }

        string assignedIcon = Icons.Material.TwoTone.Foundation;
        if (iconType is not null)
        {
            var iconList = Utility.GetConstants(iconType);
            var fieldInfo = iconList.Find(obj => obj.Name.Equals(iconIdentifier[2]));
            assignedIcon = fieldInfo.GetValue(null).ToString(); }
        //Console.WriteLine($"IconTypeConverter After : {assignedIcon}");
        return assignedIcon;
        //return Enum.Parse<string>(reader.GetString(), true);
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}



public class ColorConverter : JsonConverter<Color>
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