using Blazor.Shared.FormGenerator.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.UI.FormGenerator.Components;
internal class ComponentBase
{
    [Parameter]
    public required Field? Fields { get; set; }

    private string? CurrentValue { get; set; }
    [Parameter]
    public required string? FormData
    {
        get => CurrentValue;
        set
        {
            if (CurrentValue != value)
            {
                CurrentValue = value;
                FormDataChanged.InvokeAsync(CurrentValue);
            }
        }
    }

    [Parameter]
    public EventCallback<string?> FormDataChanged { get; set; }
}

