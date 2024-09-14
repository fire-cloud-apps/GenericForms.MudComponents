using MudBlazor;

namespace Blazor.Shared.FormGenerator.Models;

public class FormBuilder
{
    public string Card { get; set; }
    public string Style { get; set; }
    public int Elevation { get; set; }
    public int Spacing { get; set; } = 2;
    public bool Outlined { get; set; }
    public string Class { get; set; }
    public GridPosition GridPosition { get; set; }
    public Header Header { get; set; }
    public Footer Footer { get; set; }

    public List<Field> Fields { get; set; }
}