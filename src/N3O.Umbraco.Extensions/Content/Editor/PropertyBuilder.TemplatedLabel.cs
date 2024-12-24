using N3O.Umbraco.Extensions;
using Newtonsoft.Json;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Content;

public class TemplatedLabelPropertyBuilder : PropertyBuilder {
    public TemplatedLabelPropertyBuilder(IContentTypeService contentTypeService) : base(contentTypeService) { }
    
    public void Set(object value) {
        Value = value.IfNotNull(JsonConvert.SerializeObject);
    }
}
