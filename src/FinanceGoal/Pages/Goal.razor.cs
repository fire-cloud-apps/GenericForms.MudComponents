using Blazor.Shared.FormGenerator.Models;
using Blazor.UI.FormGenerator;
using System.Net.Http.Json;
using static FinanceGoal.Pages.Home;
using System.Text.Json;
using FinanceGoal.Helper;

namespace FinanceGoal.Pages;
public partial class Goal
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
        #region Goal Details
        _formBuilders = await Http.GetFromJsonAsync<FormBuilder[]>("forms/goal-form.json?v=" + Guid.NewGuid().ToString());

        //Detail Panel
        var detailPanel = _formBuilders?.Where(card => card.Card == "GoalDetails").FirstOrDefault();
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

                DynamicMudForm.AttachTextChangeEvent(_formBuilders, cardTitle, "ImageSVG", async (value) =>
                {
                    //Console.WriteLine($"Custom Text Change Event Triggered: {value}");
                    var field = DynamicMudForm.GetField(_formBuilders, cardTitle, "ImageSVG");
                    if (field is not null) // Ensure field is not null before accessing its properties
                    {
                        field.ImageSrc = value;
                        Console.WriteLine($"Image SVG Assigned.");
                        StateHasChanged();
                    }
                });
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
        Console.WriteLine("Submit Button Click - Iteration - 5");
        await _dynamicMudFrom.GetMudFrom().Validate();

        if (_dynamicMudFrom.GetMudFrom().IsValid)
        {
            var jsonData = _dynamicMudFrom.GetFormData();
            Logger.LogInformation($"jsonData {jsonData} ");
            var goalData = JsonSerializer.Deserialize<GoalModel>(jsonData);
            var goalJsonData = JsonSerializer.Serialize(goalData);
            Logger.LogInformation($"goalJsonData {goalJsonData} ");

            var jwt = await LocalStorage.GetItemAsStringAsync("access_token");
            if (string.IsNullOrEmpty(jwt))
            {
                Logger.LogError("JWT Token is null or empty.");
                return;
            }
            else
            {
                jwt = jwt.Replace("\"", "");
            }
            var result = await GoalHttp.Insert(goalData, jwt);
            Logger.LogInformation($"Result: {result} ");
        }
    }
    #endregion
}

