using System.Text.Json.Serialization;

namespace N3O.Umbraco.Search.Typesense.Models;

public abstract class SearchDocument : Value {
    [JsonPropertyName("id")]
    public string Id { get;  set; }
}