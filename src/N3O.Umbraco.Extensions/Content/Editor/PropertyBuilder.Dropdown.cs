using Newtonsoft.Json;

namespace N3O.Umbraco.Content {
    public class DropdownPropertyBuilder : PropertyBuilder {
        public void Set(string value) {
            Value = JsonConvert.SerializeObject(new[] {value});
        }
    }
}