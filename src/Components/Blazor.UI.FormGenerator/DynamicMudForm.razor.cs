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
    public  FormBuilder[] FormBuilders { get; set; }
    
    [Parameter]
    public GlobalBuilderSettings GlobalBuilderSettings { get; set; }

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
    private Dictionary<string, decimal> _formDataNumeric = new Dictionary<string, decimal>();
    private Dictionary<string, bool> _formDataBool = new Dictionary<string, bool>();
    private Dictionary<string,  DateTime> _formDataDateTime = new Dictionary<string,  DateTime>();

    #endregion
    
    #region Initialization Method

    protected override async Task OnInitializedAsync()
    {
        //@bind-Value="formData[field.FieldName]"
        if (FormBuilders is not null)
        {
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
                            Logger.LogInformation($"Form Field : {field.FieldName} - Added, {field.DefaultValue}");
                            break;
                        case InputFieldType.Select:
                        case InputFieldType.AutoComplete:
                            _formData.Add(field.FieldName, string.Empty);
                            Logger.LogInformation($"Form Field : {field.FieldName} - Added, {field.DefaultValue}");
                            break;
                        case InputFieldType.FileUpload:
                            _formData.Add(field.FieldName, string.Empty);
                            Logger.LogInformation($"Form Field : {field.FieldName} - Added, {field.DefaultValue}");
                            break;
                        case InputFieldType.CheckBox:
                        case InputFieldType.Switch:
                            _formDataBool.Add(field.FieldName, false);
                            Logger.LogInformation($"Form Field : {field.FieldName} - Added, {field.DefaultValue}");
                            break;
                        case InputFieldType.Numeric:                        
                            _formDataNumeric.Add(field.FieldName, 0);
                            Logger.LogInformation($"Form Field : {field.FieldName} - Added, {field.DefaultValue}");
                            break;
                        case InputFieldType.DateTime:
                        case InputFieldType.Date:
                            _formDataDateTime.Add(field.FieldName, DateTime.Now);
                            Logger.LogInformation($"Form Field : {field.FieldName} - Added, {field.DefaultValue}");
                            break;
                    }
                }

                JsonSerializer.Serialize(formInput.Header);
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
            _formData[item.Key] = item.Value.ToString();
            //.ToString("o"); // Using the "o" (round-trip) format specifier for DateTime
        }
        // Merge Bool data into formData
        foreach (var item in _formDataBool)
        {
            _formData[item.Key] = item.Value.ToString(); 
        }

        return JsonSerializer.Serialize(_formData);
    }

    public static bool AttachCard_EventAction(FormBuilder[] formBuilders, string cardName, 
        Func<Task?> cardAction)
    {
        bool result = false;
        //Identify Card Panel and attach 'Func<Task?>'
        var detailPanel = formBuilders.Where(card => card.Card == cardName).FirstOrDefault();
        if (detailPanel is not null)
        {
            detailPanel.Header.CardAction.ActionTrigger = cardAction;
            result = true;
        }
        return result;
    }
    
    public static bool AttachSubmitButton_EventAction(FormBuilder[] formBuilders, string cardName, 
        Func<EventArgs, Task?> cardAction)
    {
        bool result = false;
        //Identify Card Panel and attach ' Func<EventArgs, Task?> cardAction'
        var detailPanel = formBuilders.Where(card => card.Card == cardName).FirstOrDefault();
        if (detailPanel is not null)
        {
            detailPanel.Footer.Submit_Handler = cardAction;
            result = true;
        }
        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="formBuilders"></param>
    /// <param name="cardName"></param>
    /// <param name="fieldName"></param>
    /// <param name="eventAction"></param>
    /// <returns></returns>
    public static bool AutoComplete_EventAction(
        FormBuilder[] formBuilders, 
        string cardName, 
        string fieldName,
        Func<string, CancellationToken, Task<IEnumerable<string>>> eventAction)
    {
        bool result = false;
        //Identify Card Panel and attach ' Func<string, CancellationToken, Task<IEnumerable<string>>>'
        var detailPanel = formBuilders.Where(card => card.Card == cardName).FirstOrDefault();
        if (detailPanel is not null)
        {
            var autoCompleteField = detailPanel.Fields.Where(field => field.FieldName == fieldName).FirstOrDefault();
            if (autoCompleteField is not null)
            {
                autoCompleteField.AutoCompleteFunction =  eventAction;
                Console.WriteLine($"{fieldName} - AutoComplete Attached.");
            }
            result = true;
        }
        return result;
    }
    
    public static bool AttachFileUpload_EventAction(FormBuilder[] formBuilders, string cardName, string fieldName,
        Func<IBrowserFile, Task?> eventAction , ILogger<object> logger = null)
    {
        bool result = false;
        //Identify Card Panel and attach ' Func<IBrowserFile, Task?> eventAction '
        var detailPanel = formBuilders.Where(card => card.Card == cardName).FirstOrDefault();
        if (detailPanel is not null)
        {
            var autoCompleteField = detailPanel.Fields.Where(field => field.FieldName == fieldName).FirstOrDefault();
            if (autoCompleteField is not null)
            {
                autoCompleteField.FilesChangedFunction = eventAction;
                if (logger is not null)
                {
                    logger.LogInformation($"FieldName: {fieldName} - Event: {eventAction.Method.Name} Attached.");
                }
            }
            result = true;
        }
        return result;
    }
    #endregion

    #region Submit Event Trigger

    [Parameter]
    public EventCallback<string> OnSubmit { get; set; }

    private async Task SubmitClick_HandlerAsync(EventArgs args, string jsonData)
    {
        // Check if OnClick has a delegate (method assigned)
        if (OnSubmit.Equals(EventCallback.Empty))
        {
            return; // No method assigned, do nothing
        }
        Console.WriteLine("Component Form Submit Invoked.");
        await _form.Validate();
        
        if (_form.IsValid)
        {
            //Snackbar.Add("Submitted!");
            GetFormData();//Merge all FormData Inputs into one.
            string json = JsonSerializer.Serialize(_formData);
            jsonData = json;
            // Handle the JSON, like sending it to an API
            Console.WriteLine($"Dynamic Form Data as JSON: {json}");
        }
        
        await OnSubmit.InvokeAsync(jsonData);
    }

    private async Task Cancel()
    {
        _form.ResetValidation();
        _form.ResetAsync();
    }
    
    #endregion

    #region AutoComplete Event Handler
    
    /// <summary>
    /// Auto Complete Function List
    /// <para name="string"> Argument 1 </para>
    /// <para name="CancellationToken"> Argument 2 </para> 
    /// </summary>
    /*[Parameter]
    public IDictionary<string, Func<string, CancellationToken, Task<IEnumerable<string>>>> AutoCompleteFuncList
    {
        get;
        set;
    } = new Dictionary<string, Func<string, CancellationToken, Task<IEnumerable<string>>>>();
    */
    #endregion

    #region Valiation List
    [Parameter]
    public IDictionary<string, Func<string, IEnumerable<string>>> ValidationFuncList
    {
        get;
        set;
    } = new Dictionary<string, Func<string, IEnumerable<string>>>();

    /* Example Method which will be added in 'ValidationFuncLis'
     private IEnumerable<string> MaxCharacters(string ch)
    {
        if (!string.IsNullOrEmpty(ch) && 25 < ch?.Length)
            yield return "Max 25 characters";
    }*/

    #endregion
    
    #region Toggle Password
    bool isShow;
    InputType PasswordInput = InputType.Password;
    string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
    
    void PasswordToggle()
    {
        if (isShow)
        {
            isShow = false;
            PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
            PasswordInput = InputType.Password;
        }
        else {
            isShow = true;
            PasswordInputIcon = Icons.Material.Filled.Visibility;
            PasswordInput = InputType.Text;
        }
    }
    #endregion

    #region File Upload
    [Parameter] 
    public IList<IBrowserFile> FileLists { get; set; } = new List<IBrowserFile>();
    
    private int _fileCounter = 0;
    //[Parameter]
    //public Func<IReadOnlyList<IBrowserFile>, Task?> FileUpload { get; set; }
    //[Parameter]
    //public Dictionary<string, IBrowserFile> SelectedFile { get; set; } = new Dictionary<string, IBrowserFile>();
    [Parameter]
    public IDictionary<string, Func<IBrowserFile, Task?>> FileUploadFuncList
    {
        get;
        set;
    } = new Dictionary<string, Func<IBrowserFile, Task?>>();
    
    #endregion
    
}

