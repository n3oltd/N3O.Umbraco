using System;

namespace N3O.Umbraco.Search.Typesense.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class CollectionAttribute : Attribute {
    public CollectionAttribute(string name) {
        Name = name;
    }
    
    public string Name { get; }
}