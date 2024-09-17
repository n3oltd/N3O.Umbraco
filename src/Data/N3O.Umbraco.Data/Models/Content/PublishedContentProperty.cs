using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Data.Models;

public class PublishedContentProperty {
    public PublishedContentProperty(string contentTypeAlias, IPublishedProperty property) {
        ContentTypeAlias = contentTypeAlias;
        Property = property;
    }
    
    public string ContentTypeAlias { get; }
    public IPublishedProperty Property { get; }
}