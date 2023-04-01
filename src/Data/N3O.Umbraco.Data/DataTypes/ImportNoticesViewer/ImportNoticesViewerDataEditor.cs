using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Data.DataTypes;

[DataEditor(DataConstants.PropertyEditorAliases.ImportNoticesViewer,
            DataEditorName,
            "~/App_Plugins/N3O.Umbraco.Data.ImportNoticesViewer/N3O.Umbraco.Data.ImportNoticesViewer.html",
            HideLabel = false,
            ValueType = ValueTypes.Json)]
public class ImportNoticesViewerDataEditor : DataEditor {
    public const string DataEditorName = "N3O Import Notices Viewer";
    
    private readonly IIOHelper _ioHelper;
    private readonly IEditorConfigurationParser _editorConfigurationParser;

    public ImportNoticesViewerDataEditor(IDataValueEditorFactory dataValueEditorFactory,
                                         IIOHelper ioHelper,
                                         IEditorConfigurationParser editorConfigurationParser)
        : base(dataValueEditorFactory) {
        _ioHelper = ioHelper;
        _editorConfigurationParser = editorConfigurationParser;
    }

    protected override IConfigurationEditor CreateConfigurationEditor() {
        return new ImportNoticesViewerConfigurationEditor(_ioHelper, _editorConfigurationParser);
    }
}
