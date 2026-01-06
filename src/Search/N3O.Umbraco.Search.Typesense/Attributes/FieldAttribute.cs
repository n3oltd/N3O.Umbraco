using System;
using Typesense;

namespace N3O.Umbraco.Search.Typesense.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class FieldAttribute : Attribute {
    public FieldAttribute(string name,
                          FieldType type,
                          bool required,
                          bool index,
                          bool facet = false,
                          bool sort = false,
                          bool infix = false,
                          string locale = null,
                          int numberOfDimensions = 0) {
        Name = name;
        Type = type;
        Required = required;
        Index = index;
        Facet = facet;
        Sort = sort;
        Infix = infix;
        Locale = locale;
        NumberOfDimensions = numberOfDimensions;
    }
    
    public string Name { get; }
    public FieldType Type { get; }
    public bool Facet { get; set; }
    public bool Required { get; set; }
    public bool Index { get; set; }
    public bool Sort { get; set; }
    public bool Infix { get; set; }
    public string Locale { get; set; }
    public int NumberOfDimensions { get; set; }
}