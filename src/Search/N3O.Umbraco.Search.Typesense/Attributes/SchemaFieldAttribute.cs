using System;
using Typesense;

namespace N3O.Umbraco.Search.Typesense.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class SchemaFieldAttribute : Attribute {
    public string Name { get; set; }
    public FieldType Type { get; set; }
    public bool? Facet { get; set; }
    public bool? Optional { get; set; }
    public bool? Index { get; set; }
    public bool? Sort { get; set; }
    public bool? Infix { get; set; }
    public string? Locale { get; set; }
    public int? NumberOfDimensions { get; set; }
    public AutoEmbeddingConfig? Embed { get; set; }
}