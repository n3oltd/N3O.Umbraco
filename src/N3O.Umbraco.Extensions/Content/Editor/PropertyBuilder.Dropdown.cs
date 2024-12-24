using Newtonsoft.Json;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Content;

public class DropdownPropertyBuilder : PropertyBuilder {
    public DropdownPropertyBuilder(IContentTypeService contentTypeService) : base(contentTypeService) { }
    
    public void Set(string value) {
        Value = JsonConvert.SerializeObject(new[] { value });
    }
}
