using N3O.Umbraco.DataTypes;
using N3O.Umbraco.Uploader.DataTypes;

namespace N3O.Umbraco.Uploader.Extensions;

public static class DataTypeEditorExtensions {
    public static UploaderDataTypeDesigner NewUploader(this IDataTypeEditor editor, string name) {
        return editor.New<UploaderDataTypeDesigner>(name);
    }
}
