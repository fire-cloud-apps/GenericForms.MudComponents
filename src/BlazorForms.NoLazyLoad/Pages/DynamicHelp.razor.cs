using System.Net;
using System.Net.Http.Json;
using Blazor.UI.FormGenerator;
using Blazor.UI.FormGenerator.Builders;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace BlazorForms.NoLazyLoad.Pages;

public partial class DynamicHelp : ComponentBase
{
    private DynamicMudForm _dynamicMudFrom;
    #region Initialization
    protected override async Task OnInitializedAsync()
    {
        await AllControlDetails();
        //await StudentDetails();
        //await GuardianDetails();
        //await AttachmentDetails();
        //await CourseHistoryDetails();
    }

    #endregion
    
    #region Student Profile Details 
    FormBuilder[] _formAllControlBuilders;
    private async Task AllControlDetails()
    {
        #region All Controls
        _formAllControlBuilders = await Http.GetFromJsonAsync<FormBuilder[]>($"forms/all-controls.json?v={DateTime.Now.Ticks}" );
        
        if (_formAllControlBuilders is not null)
        {
            DynamicMudForm.AttachCard_EventAction(_formAllControlBuilders, "TextBox", TextBox_CardActionClick);
            DynamicMudForm.AttachSubmitButton_EventAction(_formAllControlBuilders, "TextBox", SubmitButton_Click);
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
        _dynamicMudFrom._form.Validate();
    }
}