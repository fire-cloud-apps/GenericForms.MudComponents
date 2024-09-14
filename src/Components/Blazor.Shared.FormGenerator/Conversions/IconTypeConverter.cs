using System.Text.Json;
using System.Text.Json.Serialization;
using MudBlazor;

namespace Blazor.Shared.FormGenerator.Conversions;


public class IconTypeConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        //Console.WriteLine($"IconTypeConverter Before : {value}");
        var iconIdentifier = value.Split('.');

        Type iconType = null;
        switch (iconIdentifier[0])//Custom or Material
        {
            case "Material":
                default:
                switch (iconIdentifier[1])
                {
                    case "Filled":
                        default:
                        iconType = typeof(MudBlazor.Icons.Material.Filled);
                        break;
                    case "Outlined":
                        iconType = typeof(MudBlazor.Icons.Material.Outlined);
                        break;
                    case "Rounded":
                        iconType = typeof(MudBlazor.Icons.Material.Rounded);
                        break;
                    case "Sharp":
                        iconType = typeof(MudBlazor.Icons.Material.Sharp);
                        break;
                    case "TwoTone":
                        iconType = typeof(MudBlazor.Icons.Material.TwoTone);
                        break;
                }
                break;
            case "Custom":
                switch (iconIdentifier[1])
                {
                    case "Brands":
                    default:
                        iconType = typeof(Icons.Custom.Brands);
                        break;
                    case "Uncategorized":
                        iconType = typeof(Icons.Custom.Uncategorized);
                        break;
                    case "FileFormats":
                        iconType = typeof(Icons.Custom.FileFormats);
                        break;
                }
                break;
            case "UserDefined":
                break;
        }

        string assignedIcon = Icons.Material.TwoTone.Foundation;
        if (iconType is not null)
        {
            var iconList = Utility.GetConstants(iconType);
            var fieldInfo = iconList.Find(obj => obj.Name.Equals(iconIdentifier[2]));
            assignedIcon = fieldInfo.GetValue(null).ToString(); }
        //Console.WriteLine($"IconTypeConverter After : {assignedIcon}");
        return assignedIcon;
        //return Enum.Parse<string>(reader.GetString(), true);
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
