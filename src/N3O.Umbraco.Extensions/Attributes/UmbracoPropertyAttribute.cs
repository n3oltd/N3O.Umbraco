using System;

namespace N3O.Umbraco.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class UmbracoPropertyAttribute : Attribute {
    public UmbracoPropertyAttribute(string propertyAlias) {
        PropertyAlias = propertyAlias;
    }

    public string PropertyAlias { get; }
}
