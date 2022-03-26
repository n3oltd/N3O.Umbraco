namespace N3O.Umbraco.Content {
    public class TogglePropertyBuilder : PropertyBuilder {
        public void Set(bool? value) {
            Value = value;
        }
    }
}