using N3O.Umbraco.EditorJs.Attributes;
using N3O.Umbraco.Extensions;
using System;
using System.Reflection;

namespace N3O.Umbraco.EditorJs.Extensions;

public static class TypeExtensions {
    public static string GetTuneId(this Type type) {
        var tuneAttribute = type.GetCustomAttribute<TuneAttribute>();

        if (tuneAttribute == null) {
            throw new Exception($"Type {type.GetFriendlyName().Quote()} is missing a required {nameof(TuneAttribute)} attribute");
        }

        return tuneAttribute.Id;
    }
}