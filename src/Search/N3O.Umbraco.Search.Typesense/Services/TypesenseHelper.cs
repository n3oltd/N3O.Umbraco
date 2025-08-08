using N3O.Umbraco.Extensions;
using N3O.Umbraco.Search.Typesense.Attributes;
using System;
using System.Reflection;

namespace N3O.Umbraco.Search.Typesense;

public static class TypesenseHelper {
    public static string GetCollectionName<TDocument>() {
        var attribute = typeof(TDocument).GetCustomAttribute<CollectionAttribute>();

        if (attribute == null) {
            throw new Exception($"Type {typeof(TDocument).GetFriendlyName()} is missing a required {nameof(CollectionAttribute)}");
        }

        return attribute.Name;
    }
    
    public static int GetCollectionVersion<TDocument>() {
        var attribute = typeof(TDocument).GetCustomAttribute<CollectionAttribute>();

        if (attribute == null) {
            throw new Exception($"Type {typeof(TDocument).GetFriendlyName()} is missing a required {nameof(CollectionAttribute)}");
        }

        return attribute.Version;
    }
}