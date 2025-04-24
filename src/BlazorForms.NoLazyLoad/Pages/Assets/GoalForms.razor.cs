using Blazor.Shared.FormGenerator.Models;
using Blazor.UI.FormGenerator;
using System.Net.Http.Json;

namespace BlazorForms.NoLazyLoad.Pages.Assets;
public partial class GoalForms
{
    #region Initialization
    private DynamicMudForm _dynamicMudFrom;
    protected override async Task OnInitializedAsync()
    {
        await GoalDetails();
    }

    #endregion

    #region Goal Profile Details 
    FormBuilder[] _formBuilders;
    string cardTitle = "GoalDetails";
    private async Task GoalDetails()
    {
        #region Student Details
        _formBuilders = await Http.GetFromJsonAsync<FormBuilder[]>("forms/assets/goal-entry.json?v=" + Guid.NewGuid().ToString());

        //Detail Panel
        var detailPanel = _formBuilders.Where(card => card.Card == "GoalDetails").FirstOrDefault();
        if (detailPanel is not null)
        {
            detailPanel.Header.CardAction.ActionTrigger = async () =>
            {
                Console.WriteLine("Triggered the Counter Action");
            };

            if (_formBuilders is not null)
            {
                DynamicMudForm.AttachCard_EventAction(_formBuilders, cardTitle, CardActionClick);
                DynamicMudForm.AttachSubmitButton_EventAction(_formBuilders, cardTitle, SubmitButton_Click);
            }
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

