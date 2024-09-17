using Microsoft.AspNetCore.Components.Forms;

namespace Blazor.Shared.FormGenerator;

public class GlobalBuilderSettings
{
    public Dictionary<string, IBrowserFile> SelectedFile { get; set; } = new Dictionary<string, IBrowserFile>();

    public bool IsDebug { get; set; } = false;
}
