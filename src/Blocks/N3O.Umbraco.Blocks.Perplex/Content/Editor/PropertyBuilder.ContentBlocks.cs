using N3O.Umbraco.Content;
using Newtonsoft.Json;
using Perplex.ContentBlocks.PropertyEditor.ModelValue;

namespace N3O.Umbraco.Blocks.Perplex;

public class ContentBlocksPropertyBuilder : PropertyBuilder {
    public void Set(ContentBlocksModelValue modelValue) {
        Value = JsonConvert.SerializeObject(modelValue);
    }
}
