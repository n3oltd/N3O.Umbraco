using System;
using System.Reflection;

namespace N3O.Umbraco.Extensions;

public static class PropertyInfoExtensions {
    public static bool HasAttribute<TAttribute>(this PropertyInfo propertyInfo) where TAttribute : Attribute {
        return propertyInfo.GetCustomAttribute<TAttribute>() != null;
    }

    public static bool HasAttribute(this PropertyInfo propertyInfo, Type attributeType) {
        return propertyInfo.GetCustomAttribute(attributeType) != null;
    }

    public static bool IsIndexer(this PropertyInfo propertyInfo) {
        return propertyInfo.GetIndexParameters().HasAny();
    }
}
