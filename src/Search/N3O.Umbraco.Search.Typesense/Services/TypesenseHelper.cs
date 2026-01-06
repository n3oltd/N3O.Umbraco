using N3O.Umbraco.Extensions;
using N3O.Umbraco.Search.Typesense.Attributes;
using N3O.Umbraco.Search.Typesense.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Typesense;

namespace N3O.Umbraco.Search.Typesense;

public static class TypesenseHelper {
    private static readonly Dictionary<Type, CollectionInfo> CollectionsMap = new();

    public static IReadOnlyList<CollectionInfo> GetAllCollections() => CollectionsMap.Values.ToList();
    
    public static CollectionInfo GetCollection<TDocument>() => CollectionsMap.GetValueOrDefault(typeof(TDocument));
    
    public static void RegisterCollectionFor<TDocument>(params string[] contentTypeAliases) {
        var attribute = typeof(TDocument).GetCustomAttribute<CollectionAttribute>();

        if (attribute == null) {
            throw new Exception($"Type {typeof(TDocument).GetFriendlyName()} is missing a required {nameof(CollectionAttribute)}");
        }

        var fields = GetFields<TDocument>();
        var collectionInfo = new CollectionInfo(new CollectionName(attribute.Name),
                                                attribute.Version,
                                                contentTypeAliases,
                                                fields);

        CollectionsMap[typeof(TDocument)] = collectionInfo;
    }

    private static IEnumerable<Field> GetFields<TDocument>() {
        var fields = new List<Field>();
        
        var attributes = typeof(TDocument).GetProperties()
                                          .Select(x => x.GetCustomAttribute<FieldAttribute>())
                                          .ExceptNull()
                                          .ToList();

        foreach (var attribute in attributes) {
            var field = new Field(attribute.Name,
                                  attribute.Type,
                                  attribute.Facet,
                                  !attribute.Required,
                                  attribute.Index,
                                  attribute.Sort,
                                  attribute.Infix,
                                  attribute.Locale,
                                  attribute.NumberOfDimensions);

            fields.Add(field);
        }

        return fields;
    }
}