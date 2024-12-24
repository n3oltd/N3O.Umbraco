using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Content;

public class TextareaPropertyBuilder : PropertyBuilder {
    public TextareaPropertyBuilder(IContentTypeService contentTypeService) : base(contentTypeService) { }
    
    public void Set(string value) {
        Value = value;
    }
}
