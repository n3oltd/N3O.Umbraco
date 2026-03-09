using N3O.Umbraco.Search.Typesense.Attributes;
using NodaTime;
using Typesense;

namespace N3O.Umbraco.Search.Typesense.Models;

[Collection("pages")]
public class PageDocument : SearchDocument {
    [Field("timestamp", FieldType.String, true, true)]
    public Instant Timestamp { get; set; }
    
    [Field("content", FieldType.String, true, true)]
    public string Content { get; set; }
    
    [Field("description", FieldType.String, true, true)]
    public string Description { get; set; }
    
    [Field("title", FieldType.String, true, true)]
    public string Title { get; set; }
    
    [Field("url", FieldType.String, true, true)]
    public string Url { get; set; }
}
