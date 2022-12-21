using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cells.DataTypes;

[DataEditor(CellsConstants.PropertyEditorAlias,
            "N3O Cells",
            "~/App_Plugins/N3O.Umbraco.Cells/N3O.Umbraco.Cells.html",
            HideLabel = false,
            ValueType = "JSON")]
public class CellsDataEditor : DataEditor {
    private readonly IIOHelper _ioHelper;
    private readonly IEditorConfigurationParser _editorConfigurationParser;

    public CellsDataEditor(IDataValueEditorFactory dataValueEditorFactory,
                           IIOHelper ioHelper,
                           IEditorConfigurationParser editorConfigurationParser)
        : base(dataValueEditorFactory) {
        _ioHelper = ioHelper;
        _editorConfigurationParser = editorConfigurationParser;
    }

    protected override IConfigurationEditor CreateConfigurationEditor() {
        return new CellsConfigurationEditor(_ioHelper, _editorConfigurationParser);
    }
}
