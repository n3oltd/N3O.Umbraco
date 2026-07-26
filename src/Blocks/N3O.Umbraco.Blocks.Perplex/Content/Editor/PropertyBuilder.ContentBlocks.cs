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

    // Perplex reads the stored value with Umbraco's IJsonSerializer, so it is written with the same one.
    public void Set(ContentBlocksValue modelValue) {
        Value = _jsonSerializer.Serialize(modelValue);
    }

    // ContentBlocksValue does not represent every stored shape, so already-serialised JSON is written through
    // as-is rather than round-tripped through it.
    public void SetJson(string json) {
        Value = json;
    }
}
