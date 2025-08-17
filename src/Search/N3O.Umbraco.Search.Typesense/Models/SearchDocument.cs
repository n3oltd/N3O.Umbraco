using N3O.Umbraco.Search.Typesense.Attributes;
using System;
using Typesense;

namespace N3O.Umbraco.Search.Typesense.Models;

public abstract class SearchDocument : Value {
    [FieldProperty("id", FieldType.String)]
    public Guid Id { get; set; }
}