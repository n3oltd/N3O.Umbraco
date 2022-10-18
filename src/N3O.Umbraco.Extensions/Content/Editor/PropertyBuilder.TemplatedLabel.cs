using N3O.Umbraco.Extensions;
using Newtonsoft.Json;

namespace N3O.Umbraco.Content;

public class TemplatedLabelPropertyBuilder : PropertyBuilder {
    public void Set(object value) {
        Value = value.IfNotNull(JsonConvert.SerializeObject);
    }
}
