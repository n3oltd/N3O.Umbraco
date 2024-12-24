using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Content;

public class TextBoxPropertyBuilder : PropertyBuilder {
    public TextBoxPropertyBuilder(IContentTypeService contentTypeService) : base(contentTypeService) { }
    
    public void Set(string value) {
        Value = value;
    }
}
