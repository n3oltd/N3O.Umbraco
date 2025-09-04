using System;

namespace N3O.Umbraco.Search.Typesense.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class CollectionAttribute : Attribute {
    public CollectionAttribute(string name, int version = 1) {
        Name = name;
        Version = version;
    }

    public string Name { get; }
    public int Version { get; }
}