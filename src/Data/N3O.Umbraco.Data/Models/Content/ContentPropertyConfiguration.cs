namespace N3O.Umbraco.Data.Models;

public class ContentPropertyConfiguration {
    public ContentPropertyConfiguration(string contentTypeAlias, string propertyAlias) {
        ContentTypeAlias = contentTypeAlias;
        PropertyAlias = propertyAlias;
    }
    
    public string ContentTypeAlias { get; }
    public string PropertyAlias { get; }
}