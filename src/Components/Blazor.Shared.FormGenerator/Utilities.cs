using System.Reflection;

namespace Blazor.Shared.FormGenerator;

public static class Utility
{
    public static List<FieldInfo> GetConstants(Type type)
    {        
        FieldInfo[] fieldInfos = type.GetFields(BindingFlags.Public |
                                                BindingFlags.Static | BindingFlags.FlattenHierarchy);

        return fieldInfos.Where(fi => fi.IsLiteral && !fi.IsInitOnly).ToList();        
    }

    public static string GetConstantValue(Type type, string constantName)
    {
        string constantValue = string.Empty;
        Console.WriteLine($"Type FullName: {type.FullName} Name :{type.Name}");
        FieldInfo[] fields = type.GetFields(BindingFlags.Public | BindingFlags.Static);
        // Iterate through all fields of the class
        foreach (FieldInfo field in fields)
        {
            // Check if the field is static and of type string
            //if (field.IsStatic && field.FieldType == typeof(string))
            if (field.IsLiteral && !field.IsInitOnly)
            {                
                // Get the value of the constant
                constantValue = field.GetRawConstantValue().ToString(); //(string)field.GetValue(null);

                // Check if the constant value matches "Constant"
                if (constantValue == constantName)
                {
                    Console.WriteLine("Found constant: " + constantValue);
                    break;
                }
            }
        }
        return constantValue;
    }
    
}