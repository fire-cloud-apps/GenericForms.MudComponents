using System.Net.Http.Json;
using Blazor.Shared.FormGenerator.Models;
using Blazor.UI.FormGenerator;
using Microsoft.AspNetCore.Components;

namespace BlazorForms.NoLazyLoad.Pages.GenericForms;

public partial class Feedback : ComponentBase
{
    
    #region Initialization
    private DynamicMudForm _dynamicMudFrom;
    FormBuilder[] _formAllControlBuilders;
    private string jsonText = string.Empty;
    private string cardTitle = "feedback";
    
    protected override async Task OnInitializedAsync()
    {
        await ControlDetails();
    }

    #endregion
    
    #region Control Details 
    
    private async Task ControlDetails()
    {
        #region All Controls
        _formAllControlBuilders = await Http.GetFromJsonAsync<FormBuilder[]>($"forms/{cardTitle}.json?v={DateTime.Now.Ticks}" );
        jsonText = await Http.GetStringAsync($"forms/{cardTitle}.json?v={DateTime.Now.Ticks}");
        
        if (_formAllControlBuilders is not null)
        {
            DynamicMudForm.AttachCard_EventAction(_formAllControlBuilders, cardTitle, CardActionClick);
            DynamicMudForm.AttachSubmitButton_EventAction(_formAllControlBuilders, cardTitle, SubmitButton_Click);
        }

        #endregion
    }

    #endregion

    #region Event Mapper
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
            Logger.LogInformation(_dynamicMudFrom.GetFormData());
        }
    }
    #endregion
}