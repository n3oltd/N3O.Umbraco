using N3O.Umbraco.Search.Typesense.Attributes;
using Newtonsoft.Json;
using System;
using Typesense;

namespace N3O.Umbraco.Search.Typesense.Models;

public abstract class SearchDocument : Value {
    [JsonProperty("id")]
    [SchemaField(Name = "id", Type = FieldType.String)]
    public Guid Id { get;  set; }
}