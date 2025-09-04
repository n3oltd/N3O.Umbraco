using N3O.Umbraco.Search.Typesense.Attributes;
using NodaTime;
using Typesense;

namespace N3O.Umbraco.Search.Typesense.Models;

[Collection("pages")]
public class PageDocument : SearchDocument {
    [FieldProperty("timestamp", FieldType.String)]
    public Instant Timestamp { get; set; }
    
    [FieldProperty("content", FieldType.String)]
    public string Content { get; set; }
    
    [FieldProperty("description", FieldType.String)]
    public string Description { get; set; }
    
    [FieldProperty("title", FieldType.String)]
    public string Title { get; set; }
    
    [FieldProperty("url", FieldType.String)]
    public string Url { get; set; }
}
