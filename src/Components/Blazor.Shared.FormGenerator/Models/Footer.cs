
using System.Text.Json.Serialization;
using Blazor.Shared.FormGenerator.Conversions;
using MudBlazor;
using Color = MudBlazor.Color;

namespace Blazor.Shared.FormGenerator.Models;

public class Footer
{
    public bool Enable { get; set; }
    public string CustomClass { get; set; } = "d-flex justify-end flex-grow-1 gap-2";
    
    public bool EnableCancel { get; set; }
    public string CancelText { get; set; }

    [JsonConverter(typeof(ColorTypeConverter))]
    public Color CancelColor { get; set; } = Color.Default;
    [JsonConverter(typeof(VariantTypeConverter))]

    public Variant CancelVariant { get; set; } = Variant.Text;
    
    public bool EnableSubmit { get; set; }
    public string SubmitText { get; set; }

    [JsonConverter(typeof(ColorTypeConverter))]
    public Color SubmitColor { get; set; } = Color.Default;

    [JsonConverter(typeof(VariantTypeConverter))]
    public Variant SubmitVariant { get; set; } = Variant.Filled;

    #region Event Handler

    /// To override this we should pass it from the parent contorl
    /// </summary>
    [JsonIgnore]
    public Func<Task?> Cancel_Handler { get; set; } = async () => { Console.WriteLine("'Cancel Event handler'. Default method trigger. Assign your method to perform Cancel."); };
    [JsonIgnore]
    public Func<EventArgs, Task?> Submit_Handler { get; set; } = async (e) => { Console.WriteLine("'Card Header Action' Default action Triggered. If you need to pass your method pass it from the parent controller."); };

    #endregion
}