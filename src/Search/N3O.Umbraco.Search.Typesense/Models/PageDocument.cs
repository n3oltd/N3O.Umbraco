using N3O.Umbraco.Search.Typesense.Attributes;
using NodaTime;
using Typesense;

namespace N3O.Umbraco.Search.Typesense.Models;

[Collection("pages")]
public class PageDocument : SearchDocument {
    [Field("timestamp", FieldType.String)]
    public Instant Timestamp { get; set; }
    
    [Field("content", FieldType.String)]
    public string Content { get; set; }
    
    [Field("description", FieldType.String)]
    public string Description { get; set; }
    
    [Field("title", FieldType.String)]
    public string Title { get; set; }
    
    [Field("url", FieldType.String)]
    public string Url { get; set; }
}
