using System.Text.Json.Serialization;
using Blazor.Shared.FormGenerator.Conversions;
using MudBlazor;

namespace Blazor.Shared.FormGenerator.Models;

public class CardActions
{
    public bool Enable { get; set; }
    [JsonConverter(typeof(ColorTypeConverter))]
    public Color Color { get; set; }
    //[JsonConverter(typeof(IconTypeConverter))]
    public string Icon { get; set; }
    [JsonIgnore]
    public Func<Task?> ActionTrigger { get; set; } = async () => { Console.WriteLine("'Card Header Action' Default action Triggered. If you need to pass your method pass it from the parent controller."); };
}