using System;

namespace N3O.Umbraco.Search.Typesense.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class CollectionAttribute : Attribute {
    public string Name { get; set; }
    public int Version { get; set; } = 1;
}