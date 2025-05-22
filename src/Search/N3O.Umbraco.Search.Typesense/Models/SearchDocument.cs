using System.Text.Json.Serialization;
using NodaTime;

namespace N3O.Umbraco.Search.Typesense.Models;

public class SearchDocument : Value {
    public SearchDocument(string content, string description, string title, string url, Instant timestamp) {
        Content = content;
        Description = description;
        Title = title;
        Url = url;
        Timestamp = timestamp;
    }
    
    [JsonPropertyName("content")]
    public string Content { get; }
    
    [JsonPropertyName("description")]
    public string Description { get; }
    
    [JsonPropertyName("title")]
    public string Title { get; }
    
    [JsonPropertyName("url")]
    public string Url { get; }
    
    [JsonPropertyName("timestamp")]
    public Instant Timestamp { get; }
}
