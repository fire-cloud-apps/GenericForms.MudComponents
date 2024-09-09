using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.Json;
using Blazor.UI.FormGenerator.Builders;
using Microsoft.AspNetCore.Components.Forms;

namespace Blazor.UI.FormGenerator;
public partial class DynamicMudForm
{
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
    
    #endregion
    
    #region Paramters
    /// <summary>
    /// For general purpose to identify easliy what this dynamic form is all about?
    /// </summary>
    [Parameter] public string Title { get; set; }
    //[Parameter] public FormProperty FormHeaderProperty { get; set; }
    
    [Parameter]
    public  FormProperty  HeaderProperty { get; set; }
    
    [Parameter]
    public FormBuilderInputs[] FormBuilderInput { get; set; }
    #endregion

    #region  Initialization Method

    protected override async Task OnInitializedAsync()
    {
       
        //@bind-Value="formData[field.FieldName]"
        if (FormBuilderInput is not null)
        {
            foreach (var formInput in FormBuilderInput)
            {
                switch (formInput.InputType)
                {
                    case InputFieldType.Text:
                    case InputFieldType.MultiLine:
                    case InputFieldType.Email:
                    case InputFieldType.Password:
                    case InputFieldType.Radio:
                    case InputFieldType.Time:
                    default:
                        formData.Add(formInput.FieldName, formInput.DefaultValue);
                        Console.WriteLine($"Form Field : {formInput.FieldName} - Added");
                        break;
                    case InputFieldType.Select:
                    case InputFieldType.AutoComplete:
                        formData.Add(formInput.FieldName, string.Empty);
                        break;
                    case InputFieldType.FileUpload:
                        formData.Add(formInput.FieldName, string.Empty);
                        break;
                    case InputFieldType.CheckBox:
                    case InputFieldType.Switch:
                        formDataBool.Add(formInput.FieldName, false);
                        break;
                    case InputFieldType.Numeric:                        
                        formDataNumeric.Add(formInput.FieldName, 0);
                        break;
                    case InputFieldType.DateTime:
                    case InputFieldType.Date:
                        formDataDateTime.Add(formInput.FieldName, DateTime.Now);
                        break;
                }
            }
        }
        
    }
    
    #endregion
    
    #region Helper Methods
    public Dictionary<string, string> GetFormData()
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

        return formData;
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
    [Parameter]
    public Func<IReadOnlyList<IBrowserFile>, Task?> FileUpload { get; set; }
    
    
    #endregion
}

