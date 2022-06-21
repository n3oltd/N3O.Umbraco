using System;

namespace N3O.Umbraco.Attributes;

public class NameAttribute : Attribute {
    public NameAttribute(string name) {
        Name = name;
    }

    public string Name { get; }
}
