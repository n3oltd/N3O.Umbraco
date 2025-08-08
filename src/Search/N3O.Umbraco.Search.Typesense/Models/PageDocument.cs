using N3O.Umbraco.Search.Typesense.Attributes;
using System.Text.Json.Serialization;

namespace N3O.Umbraco.Search.Typesense.Models;

[Collection(Name = "Pages")]
public class PageDocument : SearchDocument {
    [JsonPropertyName("timestamp")]
    public string Timestamp { get; set; }
    
    [JsonPropertyName("content")]
    public string Content { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; }
    
    [JsonPropertyName("title")]
    public string Title { get; set; }
    
    [JsonPropertyName("url")]
    public string Url { get; set; }
}
