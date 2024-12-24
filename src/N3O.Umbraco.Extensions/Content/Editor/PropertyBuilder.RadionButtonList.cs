using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Content;

public class RadioButtonListPropertyBuilder : PropertyBuilder {
    public RadioButtonListPropertyBuilder(IContentTypeService contentTypeService) : base(contentTypeService) { }
    
    public void Set(string value) {
        Value = value;
    }
}
