using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace N3O.Umbraco.Cloud.Extensions;

public static class EnumExtensions {
    public static string ToEnumString<T>(this T value) where T : struct, Enum {
        var enumType = typeof(T);
        var name = Enum.GetName(enumType, value);
        var enumMemberAttribute = enumType.GetField(name).GetCustomAttribute<EnumMemberAttribute>();
        
        return enumMemberAttribute?.Value;
    }
}