using System.Net.Mail;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.Json;
using Blazor.UI.FormGenerator.Builders;
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

    #region  Parameter Events for Trigger & Action

    //[Parameter] public Func<Task?> CardHeader_ActionTrigger { get; set; } = async () => { Console.WriteLine("'Card Header Action' Default action Triggered. If you need to pass your method pass it from the parent controller."); };

    //[Parameter] public Func<Task?> Cancel_Handler { get; set; } = async () => { Console.WriteLine("'Cancel Event handler'. Default method trigger. Assign your method to perform Cancel."); };
    //[Parameter] public Func<EventArgs, Task?> Submit_Handler { get; set; } = async (e) => { Console.WriteLine("'Card Header Action' Default action Triggered. If you need to pass your method pass it from the parent controller."); };
    #endregion
    
   
    #region Global Variables
    public MudForm _form;
    bool success;
    string[] errors = { };
    private Dictionary<string, string> formData = new Dictionary<string, string>();
    private Dictionary<string, IList<SelectionItems>> formItemArrayData = new Dictionary<string, IList<SelectionItems>>();
    private Dictionary<string, decimal> formDataNumeric = new Dictionary<string, decimal>();
    private Dictionary<string, bool> formDataBool = new Dictionary<string, bool>();
    private Dictionary<string,  DateTime> formDataDateTime = new Dictionary<string,  DateTime>();
    public Dictionary<string, IReadOnlyList<IBrowserFile>> formDataFile = new Dictionary<string, IReadOnlyList<IBrowserFile>>();

    public Dictionary<string, MudForm> _mudFormDictionary = new Dictionary<string, MudForm>();
    
    #endregion
    
    #region Paramters

    //[Parameter] public RenderFragment CustomComponent { get; set; }

    /// <summary>
    /// For general purpose to identify easliy what this dynamic form is all about?
    /// </summary>
    [Parameter] public string Title { get; set; }
    
    [Parameter]
    public  FormProperty  HeaderProperty { get; set; }
    
    //[Parameter]
    //public FormBuilderInputs[] FormBuilderInput { get; set; }
    #endregion

    #region  Initialization Method

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
                            formData.Add(field.FieldName, field.DefaultValue);
                            Logger.LogInformation($"Form Field : {field.FieldName} - Added, {field.DefaultValue}");
                            break;
                        case InputFieldType.Select:
                        case InputFieldType.AutoComplete:
                            formData.Add(field.FieldName, string.Empty);
                            Logger.LogInformation($"Form Field : {field.FieldName} - Added, {field.DefaultValue}");
                            break;
                        case InputFieldType.FileUpload:
                            formData.Add(field.FieldName, string.Empty);
                            Logger.LogInformation($"Form Field : {field.FieldName} - Added, {field.DefaultValue}");
                            break;
                        case InputFieldType.CheckBox:
                        case InputFieldType.Switch:
                            formDataBool.Add(field.FieldName, false);
                            Logger.LogInformation($"Form Field : {field.FieldName} - Added, {field.DefaultValue}");
                            break;
                        case InputFieldType.Numeric:                        
                            formDataNumeric.Add(field.FieldName, 0);
                            Logger.LogInformation($"Form Field : {field.FieldName} - Added, {field.DefaultValue}");
                            break;
                        case InputFieldType.DateTime:
                        case InputFieldType.Date:
                            formDataDateTime.Add(field.FieldName, DateTime.Now);
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
        foreach (var item in formDataNumeric)
        {
            formData[item.Key] = item.Value.ToString();
        }

        // Merge DateTime data into formData
        foreach (var item in formDataDateTime)
        {
            formData[item.Key] = item.Value.ToString();
            //.ToString("o"); // Using the "o" (round-trip) format specifier for DateTime
        }
        // Merge Bool data into formData
        foreach (var item in formDataBool)
        {
            formData[item.Key] = item.Value.ToString(); 
        }

        return JsonSerializer.Serialize(formData);
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
            string json = JsonSerializer.Serialize(formData);
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
    /// <para name="Task<IEnumerable<string>>"> Return type </para>
    /// </summary>
    [Parameter]
    public IDictionary<string, Func<string, CancellationToken, Task<IEnumerable<string>>>> AutoCompleteFuncList
    {
        get;
        set;
    } = new Dictionary<string, Func<string, CancellationToken, Task<IEnumerable<string>>>>();
    
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

