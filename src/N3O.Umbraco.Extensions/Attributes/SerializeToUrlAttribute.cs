using System;

namespace N3O.Umbraco.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class SerializeToUrlAttribute : Attribute {
    public SerializeToUrlAttribute(string property) {
        Property = property;
    }
    
    public string Property { get; }
}
