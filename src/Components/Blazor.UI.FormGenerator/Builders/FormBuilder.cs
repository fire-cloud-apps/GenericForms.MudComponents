using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using MudBlazor;

namespace Blazor.UI.FormGenerator.Builders;

public class FormBuilder
{
    public string Card { get; set; }
    public string Style { get; set; }
    public int Elevation { get; set; }
    public int Spacing { get; set; } = 2;
    public bool Outlined { get; set; }
    public string Class { get; set; }
    public GridPosition GridPosition { get; set; }
    public Header Header { get; set; }
    public Footer Footer { get; set; }

    public List<Field> Fields { get; set; }
}
// Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
public class Avatar
{
    public bool Enable { get; set; }
    [JsonConverter(typeof(ColorConverter))]
    public Color Color { get; set; }
    [JsonConverter(typeof(IconTypeConverter))]
    public string Icon { get; set; }
    [JsonConverter(typeof(VariantTypeConverter))]
    public Variant Variant { get; set; }
}

public class CardActions
{
    public bool Enable { get; set; }
    [JsonConverter(typeof(ColorConverter))]
    public Color Color { get; set; }
    [JsonConverter(typeof(IconTypeConverter))]
    public string Icon { get; set; }
    [JsonIgnore]
    public Func<Task?> ActionTrigger { get; set; } = async () => { Console.WriteLine("'Card Header Action' Default action Triggered. If you need to pass your method pass it from the parent controller."); };
}

public class Content
{
    public bool Enable { get; set; }
    public string Title { get; set; }
    public string SubTitle { get; set; }
}

public class Field
{
    public string FieldName { get; set; }
    public string Label { get; set; }
    /// <summary>
    /// The text field will apply formatting rules or input restrictions on-the-fly while the user is typing. eg."0000 0000 0000 0000", "+(91)-(00000)-(00000)". etc.
    /// </summary>
    public string PatternMask { get; set; }
    /// <summary>
    /// This applies 'ReadOnly' ability to a control. Default is 'false'.
    /// </summary>
    public bool IsReadOnly { get; set; } = false;
    
    [JsonConverter(typeof(InputFieldTypeConverter))]
    public InputFieldType InputType { get; set; }

    public string DefaultValue { get; set; }

    [JsonConverter(typeof(VariantTypeConverter))]
    public Variant Variant { get; set; } = Variant.Filled;

    #region Text Input Specific Property

    [JsonConverter(typeof(IconTypeConverter))]
    public string AdornmentIcon { get; set; }

    [JsonConverter(typeof(AdornmentTypeConverter))]
    public Adornment Adornment { get; set; } = Adornment.Start;
    
    public string AdornmentText { get; set; }
    
    [JsonConverter(typeof(ColorConverter))]
    public Color AdornmentColor { get; set; }

    [JsonIgnore]
    public Func<string, IEnumerable<string>> ValidateInput { get; set; } = null;
    // Default function implementation
    private IEnumerable<string> MaxCharacters(string ch)
    {
        if (!string.IsNullOrEmpty(ch) && 25 < ch?.Length)
            yield return "Max 25 characters";
    }
    
    [JsonIgnore]
    public Func<string, CancellationToken, Task<IEnumerable<string>>> AutoCompleteFunction { get; set; } = null;
    
    
    //private void UploadFiles(IReadOnlyList<IBrowserFile> files)
    //IEnumerable<string> MaxCharacters(string ch)
    //async Task<IEnumerable<string>> FindCountry(string value, CancellationToken token)
    
    #endregion
    
    #region File Uplaod Fields
    [JsonIgnore]
    public Func<IBrowserFile, Task?> FilesChangedFunction { get; set; } 
    /// <summary>
    /// This indicates that, what extension is allowed as a part of file upload.
    /// Eg. ".png, .jpg" or ".pdf, .doc" etc.
    /// </summary>
    public string FileAllowed { get; set; } = string.Empty;
    #endregion
   
    
    public GridPosition GridPosition { get; set; }
    
    /// <summary>
    /// This indicates the extended 'Validation' function for a Form.
    /// </summary>
    public FormValidation Validation { get; set; } = new FormValidation()
    {
        IsRequired = false,
        RequiredError = ""
    };
    /// <summary>
    /// This is applied on the Text Input, where it requires, multiple line of items to provide input. eg. 'Description', 'Message' etc.
    /// </summary>
    public int Line { get; set; } = 3;

    #region Selection Type

    /// <summary>
    /// SelectItemList, is applicable for the 'MudSelect' and 'MudRadio' Currently in futuer if required to have multiple selection option on a control, this will be used.
    /// </summary>
    public SelectionItems[] SelectItemList { get; set; }

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
    /// Custom class which currently applies in Image control. 'rounded-lg' justify-center
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

public class Footer
{
    public bool Enable { get; set; }
    public string CustomClass { get; set; } = "d-flex justify-end flex-grow-1 gap-2";
    
    public bool EnableCancel { get; set; }
    public string CancelText { get; set; }

    [JsonConverter(typeof(ColorConverter))]
    public Color CancelColor { get; set; } = Color.Default;
    [JsonConverter(typeof(VariantTypeConverter))]

    public Variant CancelVariant { get; set; } = Variant.Text;
    
    public bool EnableSubmit { get; set; }
    public string SubmitText { get; set; }

    [JsonConverter(typeof(ColorConverter))]
    public Color SubmitColor { get; set; } = Color.Default;

    [JsonConverter(typeof(VariantTypeConverter))]
    public Variant SubmitVariant { get; set; } = Variant.Filled;

    #region Event Handler

    /// To override this we should pass it from the parent contorl
    /// </summary>
    [JsonIgnore]
    public Func<Task?> Cancel_Handler { get; set; } = async () => { Console.WriteLine("'Cancel Event handler'. Default method trigger. Assign your method to perform Cancel."); };
    [JsonIgnore]
    public Func<EventArgs, Task?> Submit_Handler { get; set; } = async (e) => { Console.WriteLine("'Card Header Action' Default action Triggered. If you need to pass your method pass it from the parent controller."); };

    #endregion
}

public class Header
{
    public bool Enable { get; set; }
    public bool Divider { get; set; }
    public Avatar Avatar { get; set; }
    public Content Content { get; set; }
    public CardActions CardAction { get; set; }
    
}


public class GlobalBuilderSettings
{
    
    public Dictionary<string, IBrowserFile> SelectedFile { get; set; } = new Dictionary<string, IBrowserFile>();

    public bool IsDebug { get; set; } = false;
}
    

