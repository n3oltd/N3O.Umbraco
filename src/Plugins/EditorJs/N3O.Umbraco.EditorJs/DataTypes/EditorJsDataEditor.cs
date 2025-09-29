using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.EditorJs.DataTypes;

[DataEditor(EditorJsConstants.PropertyEditorAlias,
            "N3O Editor JS",
            "~/App_Plugins/N3O.Umbraco.EditorJs/N3O.Umbraco.EditorJs.html",
            HideLabel = true,
            Icon = "icon-notepad",
            Group = "Rich Content",
            ValueType = ValueTypes.Json)]
public class EditorJsDataEditor : DataEditor {
    private readonly IIOHelper _ioHelper;
    private readonly IEditorConfigurationParser _editorConfigurationParser;

    public EditorJsDataEditor(IDataValueEditorFactory dataValueEditorFactory,
                              IIOHelper ioHelper,
                              IEditorConfigurationParser editorConfigurationParser)
        : base(dataValueEditorFactory) {
        _ioHelper = ioHelper;
        _editorConfigurationParser = editorConfigurationParser;
    }

    protected override IConfigurationEditor CreateConfigurationEditor() {
        return new EditorJsConfigurationEditor(_ioHelper, _editorConfigurationParser);
    }
}
