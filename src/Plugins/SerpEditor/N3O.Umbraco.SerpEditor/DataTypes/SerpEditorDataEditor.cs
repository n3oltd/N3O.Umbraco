using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.SerpEditor.DataTypes;

[DataEditor(SerpEditorConstants.PropertyEditorAlias,
            "N3O SERP Editor",
            "~/App_Plugins/N3O.Umbraco.SerpEditor/N3O.Umbraco.SerpEditor.html",
            HideLabel = false,
            ValueType = "JSON")]
public class SerpEditorDataEditor : DataEditor {
    private readonly IIOHelper _ioHelper;
    private readonly IEditorConfigurationParser _editorConfigurationParser;

    public SerpEditorDataEditor(IDataValueEditorFactory dataValueEditorFactory,
                                IIOHelper ioHelper,
                                IEditorConfigurationParser editorConfigurationParser)
        : base(dataValueEditorFactory) {
        _ioHelper = ioHelper;
        _editorConfigurationParser = editorConfigurationParser;
    }

    protected override IConfigurationEditor CreateConfigurationEditor() {
        return new SerpEditorConfigurationEditor(_ioHelper, _editorConfigurationParser);
    }
}
