using System.Globalization;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.Json;
using Blazor.Shared.FormGenerator;
using Blazor.Shared.FormGenerator.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;

namespace Blazor.UI.FormGenerator;
public partial class DynamicMudForm
{
    #region Parameters

    [Parameter]
    public required FormBuilder[] FormBuilders { get; set; }

    [Parameter] public GlobalBuilderSettings GlobalBuilderSettings { get; set; } = new GlobalBuilderSettings();    

    #endregion

    #region Property
    MudForm _form;
    bool _success;
    string[] _errors = { };
    
    /// <summary>
    /// Get MudForm "ref" Object to be used in Parent form to validate form and get form data.
    /// </summary>
    /// <returns>returns 'MudForm'</returns>
    public MudForm GetMudFrom()
    {
        return _form;
    }

    /// <summary>
    /// Get Validation Error message
    /// </summary>
    /// <returns>Error message as string[] </returns>
    public string[] GetValidationError()
    {
        return _errors;
    }
    

    #endregion
    
    #region Global Variables
    private Dictionary<string, string> _formData = new Dictionary<string, string>();
    private Dictionary<string, IList<SelectionItems>> _formItemArrayData = new Dictionary<string, IList<SelectionItems>>();
    private Dictionary<string, IReadOnlyCollection<string>> _formReadOnlySelectData = new Dictionary<string, IReadOnlyCollection<string>>();
    private Dictionary<string, decimal?> _formDataNumeric = new Dictionary<string, decimal?>();
    private Dictionary<string, bool> _formDataBool = new Dictionary<string, bool>();
    private Dictionary<string,  DateTime> _formDataDateTime = new Dictionary<string,  DateTime>();
    
    #endregion
    
    #region Initialization Method

    protected override async Task OnInitializedAsync()
    {
        //@bind-Value="formData[field.FieldName]"
        if(FormBuilders is null)
        {
            Logger.LogWarning("FormBuilders Property is null");
            return;
        }
        foreach (var formInput in FormBuilders)
        {
            foreach (var field in formInput.Fields)
            {
                switch (field.InputType)
                {
                    case InputFieldType.Text:
                    case InputFieldType.MultiLine:
                    case InputFieldType.Email:
                    case InputFieldType.Password:
                    case InputFieldType.Radio:
                    case InputFieldType.Time:
                    default:
                        _formData.Add(field.FieldName, field.DefaultValue);
                        break;
                    case InputFieldType.Select:
                    case InputFieldType.AutoComplete:
                        _formData.Add(field.FieldName, string.Empty);
                        break;
                    case InputFieldType.ChipSet:
                        _formData.Add(field.FieldName, string.Empty);
                        break;
                    case InputFieldType.ChipSets:
                        _formReadOnlySelectData.Add(field.FieldName, null);
                        break;
                    case InputFieldType.FileUpload:
                        _formData.Add(field.FieldName, string.Empty);
                        break;
                    case InputFieldType.CheckBox:
                    case InputFieldType.Switch:
                        _formDataBool.Add(field.FieldName, false);
                        break;
                    case InputFieldType.Numeric:                        
                        _formDataNumeric.Add(field.FieldName, 0);
                        break;
                    case InputFieldType.DateTime:
                    case InputFieldType.Date:
                        _formDataDateTime.Add(field.FieldName, DateTime.Now);
                        break;
                }
                
                Logger.LogInformation("Form Field : {FieldFieldName} - Added, Default Value : {FieldDefaultValue}, Input Type : {FieldInputType}", field.FieldName, field.DefaultValue, field.InputType.ToString());
            }
        }
    }
    
    #endregion
    
    #region Helper Methods
    public string GetFormData()
    {
        // Merge numeric data into formData
        foreach (var item in _formDataNumeric)
        {
            _formData[item.Key] = item.Value.ToString();
        }

        // Merge DateTime data into formData
        foreach (var item in _formDataDateTime)
        {
            _formData[item.Key] = item.Value.ToString(CultureInfo.CurrentCulture);
            //.ToString("o"); // Using the "o" (round-trip) format specifier for DateTime
        }
        // Merge Bool data into formData
        foreach (var item in _formDataBool)
        {
            _formData[item.Key] = item.Value.ToString(); 
        }
        // Merge IReadOnlyCollection<string> data into formDate 
        foreach (var item in _formReadOnlySelectData)
        {
            //here is the problem.
            var items = item.Value.ToArray();
            var itemValue = string.Join(", ", items);
            _formData[item.Key] = itemValue;
        }
        
        return JsonSerializer.Serialize(_formData);
    }

    #endregion

    #region Submit Event Trigger

    [Parameter]
    public EventCallback<string> OnSubmit { get; set; }

    private async Task SubmitClick_HandlerAsync(EventArgs args, string jsonData)
    {
        Console.WriteLine("Component Form Submit Invoked.");
        await _form.Validate();
        
        if (_form.IsValid)
        {
            //Snackbar.Add("Submitted!");
            GetFormData();//Merge all FormData Inputs into one.
            var option = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
            var json = JsonSerializer.Serialize(_formData, option);
            // Handle the JSON, like sending it to an API
            Console.WriteLine($"Dynamic Form Data as JSON: {json}");
        }
        
       // await OnSubmit.InvokeAsync(jsonData);
    }

    private async Task Cancel()
    {
        _form.ResetValidation();
        _form.ResetAsync();
    }
    
    #endregion

    #region Valiation List
    //[Parameter]
    /*public IDictionary<string, Func<string, IEnumerable<string>>> ValidationFuncList
    {
        get;
        set;
    } = new Dictionary<string, Func<string, IEnumerable<string>>>();
    */
    /* Example Method which will be added in 'ValidationFuncLis'
     private IEnumerable<string> MaxCharacters(string ch)
    {
        if (!string.IsNullOrEmpty(ch) && 25 < ch?.Length)
            yield return "Max 25 characters";
    }*/

    #endregion
    
    

    #region File Upload
    [Parameter] 
    public IList<IBrowserFile> FileLists { get; set; } = new List<IBrowserFile>();
    private int _fileCounter = 0;
    [Parameter]
    public IDictionary<string, Func<IBrowserFile, Task?>> FileUploadFuncList
    {
        get;
        set;
    } = new Dictionary<string, Func<IBrowserFile, Task?>>();
    
    #endregion
    
}

