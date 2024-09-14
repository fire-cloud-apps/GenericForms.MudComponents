using System.Net.Http.Json;
using Blazor.Shared.FormGenerator;
using Blazor.Shared.FormGenerator.Models;
using Blazor.UI.FormGenerator;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorForms.NoLazyLoad.Pages.Controls;

public partial class FileUploadControl : ComponentBase
{
        #region Initialization
    
    private DynamicMudForm _dynamicMudFrom;
    protected override async Task OnInitializedAsync()
    {
        await AllControlDetails();
    }

    #endregion
    
    #region Control Details 
    FormBuilder[] _formAllControlBuilders;
    private string jsonText = string.Empty;
    
    private string cardTitle = "file-upload";
    private async Task AllControlDetails()
    {
        #region All Controls
        
        _formAllControlBuilders = await Http.GetFromJsonAsync<FormBuilder[]>($"json-control/{cardTitle}.json?v={DateTime.Now.Ticks}" );
        jsonText = await Http.GetStringAsync($"json-control/{cardTitle}.json?v={DateTime.Now.Ticks}");
        
        if (_formAllControlBuilders is not null)
        {
            DynamicMudForm.AttachCard_EventAction(
                _formAllControlBuilders, 
                cardTitle, 
                CardActionClick);
            
            DynamicMudForm.AttachSubmitButton_EventAction(
                _formAllControlBuilders, 
                cardTitle, 
                SubmitButton_Click);

            DynamicMudForm.AttachFileUpload_EventAction(
                _formAllControlBuilders, 
                cardTitle, 
                "SimpleAttachments", 
                UploadPhotoFile1, Logger);
            
            DynamicMudForm.AttachFileUpload_EventAction(
                _formAllControlBuilders, 
                cardTitle, 
                "RequiredAttachments", 
                UploadPhotoFile2, Logger);
        }

        #endregion
    }

    #endregion

    #region Events

    public async Task CardActionClick()
    {
        Console.WriteLine("Triggered the Card Action Clicked.");
    }

    public async Task SubmitButton_Click(EventArgs args)
    {
        Console.WriteLine("Submit Button Click");
        _dynamicMudFrom.GetMudFrom().Validate();
        if (_dynamicMudFrom.GetMudFrom().IsValid)
        {
            //Logger.LogWarning(_dynamicMudFrom.GetFormData());
            Console.WriteLine(_dynamicMudFrom.GetFormData());
        }
    }

    #endregion

    #region Attachment Files Events
    
    private GlobalBuilderSettings _globalBuilderSettings = new GlobalBuilderSettings();
    private async Task UploadPhotoFile1(IBrowserFile file)
    {
        string key = "SimpleAttachments";
        if (_globalBuilderSettings.SelectedFile.ContainsKey(key))
        {
            _globalBuilderSettings.SelectedFile.Remove(key);
        }
        _globalBuilderSettings.SelectedFile.Add(key, file);
    }
    private async Task UploadPhotoFile2(IBrowserFile file)
    {
        string key = "RequiredAttachments";
        if (_globalBuilderSettings.SelectedFile.ContainsKey(key))
        {
            _globalBuilderSettings.SelectedFile.Remove(key);
        }
        _globalBuilderSettings.SelectedFile.Add(key, file);
    }

    #endregion
    
}