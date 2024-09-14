using System.Net.Http.Json;
using Blazor.Shared.FormGenerator.Models;
using Blazor.UI.FormGenerator;
using Microsoft.AspNetCore.Components;

namespace BlazorForms.NoLazyLoad.Pages.Controls;

public partial class AutoCompleteControl : ComponentBase
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
    
    private string cardTitle = "auto-complete";
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
                cardTitle, SubmitButton_Click);
            
            DynamicMudForm.AutoComplete_EventAction(
                _formAllControlBuilders, 
                cardTitle,
                "SimpleAutoComplete", FindCountry);
            
            DynamicMudForm.AutoComplete_EventAction(
                _formAllControlBuilders, 
                cardTitle,
                "RequiredAutoComplete", FindCountry);
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
    
    #region Auto Complete

    public IDictionary<string, Func<string, CancellationToken, Task<IEnumerable<string>>>> _funcAddressList = new Dictionary<string, Func<string, CancellationToken, Task<IEnumerable<string>>>>();
    private string[] countries =
    {
        "Alabama", "Alaska", "American Samoa", "Arizona",
        "Arkansas", "California", "Colorado", "Connecticut",
        "Delaware", "District of Columbia", "Federated States of Micronesia",
        "Florida", "Georgia", "Guam", "Hawaii", "Idaho",
        "Illinois", "Indiana", "Iowa", "Kansas", "Kentucky",
        "Louisiana", "Maine", "Marshall Islands", "Maryland",
        "Massachusetts", "Michigan", "Minnesota", "Mississippi",
        "Missouri", "Montana", "Nebraska", "Nevada",
        "New Hampshire", "New Jersey", "New Mexico", "New York",
        "North Carolina", "North Dakota", "Northern Mariana Islands", "Ohio",
        "Oklahoma", "Oregon", "Palau", "Pennsylvania", "Puerto Rico",
        "Rhode Island", "South Carolina", "South Dakota", "Tennessee",
        "Texas", "Utah", "Vermont", "Virgin Island", "Virginia",
        "Washington", "West Virginia", "Wisconsin", "Wyoming",
    };
    private async Task<IEnumerable<string>> FindCountry(string value, CancellationToken token)
    {
        Console.WriteLine("Triggred Auto Complete Country.");
        // In real life use an asynchronous function for fetching data from an api.
        await Task.Delay(5, token);
        //To Test using API's https://dummyjson.com/docs/products#products-search
        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
            return countries;
        return countries.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    #endregion
}