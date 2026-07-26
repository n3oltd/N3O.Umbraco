using N3O.Umbraco.Content;
using Perplex.ContentBlocks.PropertyEditor.Value;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Blocks.Perplex;

public class ContentBlocksPropertyBuilder : PropertyBuilder {
    private readonly IJsonSerializer _jsonSerializer;

    public ContentBlocksPropertyBuilder(IContentTypeService contentTypeService, IJsonSerializer jsonSerializer)
        : base(contentTypeService) {
        _jsonSerializer = jsonSerializer;
    }

    public void Set(ContentBlocksValue modelValue) {
        Value = _jsonSerializer.Serialize(modelValue);
    }

    // Perplex reads the stored value with Umbraco's IJsonSerializer, so content blocks JSON that is already
    // serialised is written through verbatim rather than round-tripped through ContentBlocksValue, which would
    // rewrite it in the v4 shape and drop anything the model does not represent.
    public void SetJson(string json) {
        Value = json;
    }
}
