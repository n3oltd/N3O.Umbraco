using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Content;

public class BooleanPropertyBuilder : PropertyBuilder {
    public BooleanPropertyBuilder(IContentTypeService contentTypeService) : base(contentTypeService) { }
    
    public void Set(bool? value) {
        Value = value;
    }
}
