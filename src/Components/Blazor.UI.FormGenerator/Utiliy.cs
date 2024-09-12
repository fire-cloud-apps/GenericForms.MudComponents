using System.Reflection;

namespace Blazor.UI.FormGenerator;

public static class Utility
{
    public static Dictionary<string, string> IconDictionary = new Dictionary<string, string>();
    public static List<FieldInfo> GetConstants(Type type)
    {
        FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public |
                                                BindingFlags.Static | BindingFlags.FlattenHierarchy);

        return fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToList();
    }

    /*
    public static void InitializeIcons()
    {
        var iconType = typeof(MudBlazor.Icons.Material.Filled);
         var icons = GetConstants(iconType);
         foreach (var icon in icons)
         {
             IconDictionary.Add($"Filled.{icon.Name}", icon.GetValue(null).ToString());
             
             Console.WriteLine($"Filled.{icon.Name}", icon.GetValue(null).ToString());
         }
          
    } */
}