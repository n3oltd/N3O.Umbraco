using N3O.Umbraco.Search.Typesense.Attributes;
using System.Text.Json.Serialization;
using NodaTime;

namespace N3O.Umbraco.Search.Typesense.Models;

[Collection("Pages")]
public class PageDocument : Value {
    [JsonPropertyName("timestamp")]
    public Instant Timestamp { get; set; }
    
    [JsonPropertyName("content")]
    public string Content { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; }
    
    [JsonPropertyName("title")]
    public string Title { get; set; }
    
    [JsonPropertyName("url")]
    public string Url { get; set; }
}
