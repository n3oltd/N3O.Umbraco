using N3O.Umbraco.Content;
using Newtonsoft.Json;
using Perplex.ContentBlocks.PropertyEditor.ModelValue;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Blocks.Perplex;

public class ContentBlocksPropertyBuilder : PropertyBuilder {
    public ContentBlocksPropertyBuilder(IContentTypeService contentTypeService) : base(contentTypeService) { }

    public void Set(ContentBlocksModelValue modelValue) {
        Value = JsonConvert.SerializeObject(modelValue);
    }
}
