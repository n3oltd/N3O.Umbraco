using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Content;

public class TogglePropertyBuilder : PropertyBuilder {
    public TogglePropertyBuilder(IContentTypeService contentTypeService) : base(contentTypeService) { }
    
    public void Set(bool? value) {
        Value = value;
    }
}
