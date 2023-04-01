using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Data.DataTypes;

[DataEditor(DataConstants.PropertyEditorAliases.ImportDataEditor,
            DataEditorName,
            "~/App_Plugins/N3O.Umbraco.Data.ImportDataEditor/N3O.Umbraco.Data.ImportDataEditor.html",
            HideLabel = false,
            ValueType = ValueTypes.Json)]
public class ImportDataEditorDataEditor : DataEditor {
    public const string DataEditorName = "N3O Import Data Editor";
    
    private readonly IIOHelper _ioHelper;
    private readonly IEditorConfigurationParser _editorConfigurationParser;

    public ImportDataEditorDataEditor(IDataValueEditorFactory dataValueEditorFactory,
                                      IIOHelper ioHelper,
                                      IEditorConfigurationParser editorConfigurationParser)
        : base(dataValueEditorFactory) {
        _ioHelper = ioHelper;
        _editorConfigurationParser = editorConfigurationParser;
    }

    protected override IConfigurationEditor CreateConfigurationEditor() {
        return new ImportDataEditorConfigurationEditor(_ioHelper, _editorConfigurationParser);
    }
}
