using System;

namespace N3O.Umbraco.Json;

public static class JsonExtensions {
    public static T DeserializeDynamicTo<T>(this IJsonProvider jsonProvider, object obj) {
        return (T) DeserializeDynamicTo(jsonProvider, obj, typeof(T));
    }
    
    public static object DeserializeDynamicTo(this IJsonProvider jsonProvider, object obj, Type type) {
        var json = jsonProvider.SerializeObject(obj);
        var typedObj = jsonProvider.DeserializeObject(json, type);

        return typedObj;
    }
}