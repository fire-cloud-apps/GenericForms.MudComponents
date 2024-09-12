using Blazor.UI.FormGenerator.Builders;
using Microsoft.AspNetCore.Components;


namespace Blazor.UI.FormGenerator;

public partial class DynamicViews : ComponentBase
{
    #region Parameters

    [Parameter]
    public  FormBuilder[] FormBuilders { get; set; }

    #endregion

    #region  Parameter Events for Trigger & Action

    [Parameter] public Func<Task?> CardHeader_ActionTrigger { get; set; } = async () => { Console.WriteLine("'Card Header Action' Default action Triggered. If you need to pass your method pass it from the parent controller."); };

    [Parameter] public Func<Task?> Cancel_Handler { get; set; } = async () => { Console.WriteLine("'Cancel Event handler'. Default method trigger. Assign your method to perform Cancel."); };
    [Parameter] public Func<EventArgs, Task?> Submit_Handler { get; set; } = async (e) => { Console.WriteLine("'Card Header Action' Default action Triggered. If you need to pass your method pass it from the parent controller."); };
    #endregion
    
    #region Initialization

    protected override async Task OnInitializedAsync()
    {
        
    }

    #endregion
}