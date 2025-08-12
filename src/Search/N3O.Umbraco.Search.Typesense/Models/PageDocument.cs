using N3O.Umbraco.Search.Typesense.Attributes;
using Newtonsoft.Json;
using NodaTime;
using Typesense;

namespace N3O.Umbraco.Search.Typesense.Models;

[Collection(Name = "Pages", Version = 1)]
public class PageDocument : SearchDocument {
    [JsonProperty("timestamp")]
    [SchemaField(Name = "timestamp", Type = FieldType.String)]
    public Instant Timestamp { get; set; }
    
    [JsonProperty("content")]
    [SchemaField(Name = "content", Type = FieldType.String)]
    public string Content { get; set; }
    
    [JsonProperty("description")]
    [SchemaField(Name = "description", Type = FieldType.String)]
    public string Description { get; set; }
    
    [JsonProperty("title")]
    [SchemaField(Name = "title", Type = FieldType.String)]
    public string Title { get; set; }
    
    [JsonProperty("url")]
    [SchemaField(Name = "url", Type = FieldType.String)]
    public string Url { get; set; }
}
