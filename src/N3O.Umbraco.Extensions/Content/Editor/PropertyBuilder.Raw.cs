using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Content;

public class RawPropertyBuilder : PropertyBuilder {
    public RawPropertyBuilder(IContentTypeService contentTypeService) : base(contentTypeService) { }
    
    public void Set(object value) {
        Value = value;
    }
}
