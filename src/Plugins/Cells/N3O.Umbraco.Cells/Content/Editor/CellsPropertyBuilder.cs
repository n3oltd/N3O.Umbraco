using N3O.Umbraco.Content;

namespace N3O.Umbraco.Cells.Content;

public class CellsPropertyBuilder : PropertyBuilder {
    public void Set(object[][] value) {
        Value = value;
    }
}
