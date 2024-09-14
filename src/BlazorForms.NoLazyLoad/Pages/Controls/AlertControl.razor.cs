using System.Net.Http.Json;
using Blazor.Shared.FormGenerator.Models;
using Blazor.UI.FormGenerator;
using Microsoft.AspNetCore.Components;

namespace BlazorForms.NoLazyLoad.Pages.Controls;


public partial class AlertControl : ComponentBase
{
    private DynamicMudForm _dynamicMudFrom;
    
    #region Initialization
    protected override async Task OnInitializedAsync()
    {
        await AllControlDetails();
    }

    #endregion
    
    #region Text Box Control Details 
    FormBuilder[] _formAllControlBuilders;
    private string jsonText = string.Empty;
    
    private string cardTitle = "Alert";
    private async Task AllControlDetails()
    {
        #region All Controls
        _formAllControlBuilders = await Http.GetFromJsonAsync<FormBuilder[]>(
            $"json-control/{cardTitle}.json?v={DateTime.Now.Ticks}" );
        jsonText = await Http.GetStringAsync($"json-control/{cardTitle}.json?v={DateTime.Now.Ticks}");
        if (_formAllControlBuilders is not null)
        {
            DynamicMudForm.AttachCard_EventAction(_formAllControlBuilders, cardTitle, TextBox_CardActionClick);
            DynamicMudForm.AttachSubmitButton_EventAction(_formAllControlBuilders, cardTitle, SubmitButton_Click);
        }

        #endregion
    }

    #endregion

    public async Task TextBox_CardActionClick()
    {
        Console.WriteLine("Triggered the TextBox Card Action Clicked.");
    }

    public async Task SubmitButton_Click(EventArgs args)
    {
        Console.WriteLine("Submit Button Click");
        _dynamicMudFrom.GetMudFrom() .Validate();
        if (_dynamicMudFrom.GetMudFrom().IsValid)
        {
            //Logger.LogWarning(_dynamicMudFrom.GetFormData());
            Console.WriteLine(_dynamicMudFrom.GetFormData());
        }
    }
}