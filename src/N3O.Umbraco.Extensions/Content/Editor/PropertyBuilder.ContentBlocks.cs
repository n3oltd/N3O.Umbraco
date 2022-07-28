using Newtonsoft.Json;
using Perplex.ContentBlocks.PropertyEditor.ModelValue;

namespace N3O.Umbraco.Content;

public class ContentBlocksPropertyBuilder : PropertyBuilder {
    public void Set(ContentBlocksModelValue modelValue) {
        Value = JsonConvert.SerializeObject(modelValue);
    }
}
