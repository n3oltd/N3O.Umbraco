using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Content;

public class EmailPropertyBuilder : PropertyBuilder {
    public EmailPropertyBuilder(IContentTypeService contentTypeService) : base(contentTypeService) { }
    
    public void Set(string value) {
        Value = value;
    }
}
