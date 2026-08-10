using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.EditorJs.DataTypes;

[DataEditor(EditorJsConstants.PropertyEditorAlias, ValueType = ValueTypes.Json)]
public class EditorJsDataEditor : DataEditor {
    private readonly IIOHelper _ioHelper;

    public EditorJsDataEditor(IDataValueEditorFactory dataValueEditorFactory,
                              IIOHelper ioHelper)
        : base(dataValueEditorFactory) {
        _ioHelper = ioHelper;
    }

    protected override IConfigurationEditor CreateConfigurationEditor() {
        return new EditorJsConfigurationEditor(_ioHelper);
    }
}
