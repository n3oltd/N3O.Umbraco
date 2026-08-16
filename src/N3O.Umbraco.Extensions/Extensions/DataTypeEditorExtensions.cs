using N3O.Umbraco.DataTypes;

namespace N3O.Umbraco.Extensions;

public static class DataTypeEditorExtensions {
    public static BlockListDataTypeDesigner NewBlockList(this IDataTypeEditor editor, string name) {
        return editor.New<BlockListDataTypeDesigner>(name);
    }

    public static DropdownDataTypeDesigner NewDropdown(this IDataTypeEditor editor, string name) {
        return editor.New<DropdownDataTypeDesigner>(name);
    }
}
