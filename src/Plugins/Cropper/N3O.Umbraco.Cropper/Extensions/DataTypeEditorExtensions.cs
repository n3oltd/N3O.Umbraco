using N3O.Umbraco.Cropper.DataTypes;
using N3O.Umbraco.DataTypes;

namespace N3O.Umbraco.Cropper.Extensions;

public static class DataTypeEditorExtensions {
    public static CropperDataTypeDesigner NewCropper(this IDataTypeEditor editor, string name) {
        return editor.New<CropperDataTypeDesigner>(name);
    }
}
