﻿using System.Text.Json.Serialization;
using Blazor.Shared.FormGenerator.Conversions;
using MudBlazor;

namespace Blazor.Shared.FormGenerator.Models;

public class Avatar
{
    public bool Enable { get; set; }
    [JsonConverter(typeof(ColorTypeConverter))]
    public Color Color { get; set; }
    //[JsonConverter(typeof(IconTypeConverter))]
    public string Icon { get; set; }
    [JsonConverter(typeof(VariantTypeConverter))]
    public Variant Variant { get; set; }

    [JsonConverter(typeof(AvatarTypeConverter))]
    public AvatarType AvatarType { get; set; } = AvatarType.Icon;


}

public enum AvatarType
{
    Icon,
    Image
}