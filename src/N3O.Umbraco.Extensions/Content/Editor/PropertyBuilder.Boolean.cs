namespace N3O.Umbraco.Content;

public class BooleanPropertyBuilder : PropertyBuilder {
    public void Set(bool? value) {
        Value = value;
    }
}
