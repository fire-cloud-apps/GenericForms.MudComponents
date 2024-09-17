﻿using System.Text.Json.Serialization;
using Blazor.Shared.FormGenerator.Conversions;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using Color = MudBlazor.Color;

namespace Blazor.Shared.FormGenerator.Models;

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

    /// <summary>
    /// Variant types are Filled, Text, Outlined, 
    /// </summary>
    [JsonConverter(typeof(VariantTypeConverter))]
    public Variant Variant { get; set; } = Variant.Filled;

    #region Text Input Specific Property

    [JsonConverter(typeof(IconTypeConverter))]
    public string AdornmentIcon { get; set; }

    [JsonConverter(typeof(AdornmentTypeConverter))]
    public Adornment Adornment { get; set; } = Adornment.Start;
    
    public string AdornmentText { get; set; }
    
    [JsonConverter(typeof(ColorTypeConverter))]
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

    public bool MultiSelect { get; set; } = false;

    #endregion
    
    #region Title Specific Settings
    /// <summary>
    /// Indicates what style to be applied to a Text Title, eg. h1, h2, h3 etc.
    /// </summary>
    [JsonConverter(typeof(TypoTypeConverter))]
    public Typo TextStyle { get; set; }

    /// <summary>
    /// justify-start | justify-center | justify-end | justify-space-between | justify-space-around
    /// <see cref="https://mudblazor.com/utilities/justify-content#applying-conditionally"/>
    /// </summary>
    public string TitleAlign { get; set; }

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
    /// If a alert message with the Severity type indicator. 'Normal', Error, Info, Success, Warning
    /// </summary>
    [JsonConverter(typeof(SeverityTypeConverter))]
    public Severity Severity { get; set; } = Severity.Normal;

    #endregion

    [JsonConverter(typeof(SelectionModeTypeConverter))]
    public SelectionMode SelectionMode { get; set; } = SelectionMode.SingleSelection;

    [JsonConverter(typeof(ColorTypeConverter))]
    public Color Color { get; set; } = Color.Default;


}