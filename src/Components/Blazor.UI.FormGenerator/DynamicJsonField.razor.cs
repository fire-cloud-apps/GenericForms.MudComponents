using System.Text.Json;
using Blazor.Shared.FormGenerator.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace Blazor.UI.FormGenerator;

public partial class DynamicJsonField : ComponentBase
{
    bool success;
    string[] errors = { };
    MudForm form;
    private Field _field = new Field();
    private string _fieldOutput;


    private async Task AllControlDetails()
    {
        _field = new Field();
    }

    private void SubmitClick_HandlerAsync(MouseEventArgs obj)
    {
        var option = new JsonSerializerOptions()
        {
            WriteIndented = true
        };
        _fieldOutput = JsonSerializer.Serialize(_field, option);
    }
}