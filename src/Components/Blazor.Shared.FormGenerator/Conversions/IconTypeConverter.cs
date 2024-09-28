using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Logging;
using MudBlazor;

namespace Blazor.Shared.FormGenerator.Conversions;


public class IconTypeConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {

        string assignedIcon = Icons.Material.TwoTone.Foundation;
        string assignedIconx = Icons.Material.Filled.Abc ;
        try
        {
            var value = reader.GetString();//@Icons.Material.Filled.AcUnit
            if (reader.TokenType == JsonTokenType.Null)
            {
                Console.WriteLine($"reader.TokenType is null 'JsonTokenType.Null'");
                return assignedIcon;
            }
            Console.WriteLine($"IconTypeConverter Before : {value}");
            var iconIdentifier = value.Split('.');
            Console.WriteLine($"Split of Icon [0]: {iconIdentifier[0]} [1]: {iconIdentifier[1]} [2]: {iconIdentifier[2]}");
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
            
            if (iconType is not null)
            {
                /*Console.WriteLine($"iconType Found: {iconType}");
                var iconList = Utility.GetConstants(iconType);
                Console.WriteLine($"iconList Count: {iconList.Count}");
                var fieldInfo = iconList.Find(obj => obj.Name.Equals(iconIdentifier[2]));
                //MudBlazor.Icons.Material.Filled.AccessAlarm
                Console.WriteLine($"fieldInfo Name: {fieldInfo.Name}");
                assignedIcon = fieldInfo.GetValue(null).ToString();
                 */
                assignedIcon =  Utility.GetConstantValue(iconType, iconIdentifier[2]);
                Console.WriteLine($"assignedIcon Value: {assignedIcon}");
            }
            else
            {
                Console.WriteLine($"iconType Could not be found.{value}");
            }
            Console.WriteLine($"IconTypeConverter After : {assignedIcon}");
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex);
        }
        return assignedIcon;
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
