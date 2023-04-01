using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.TextResourceEditor.DataTypes;

[DataEditor(TextResourceEditorConstants.PropertyEditorAlias,
            "N3O Text Resource Editor",
            "~/App_Plugins/N3O.Umbraco.TextResourceEditor/N3O.Umbraco.TextResourceEditor.html",
            HideLabel = false,
            ValueType = ValueTypes.Json)]
public class TextResourceEditorDataEditor : DataEditor {
    private readonly IIOHelper _ioHelper;
    private readonly IEditorConfigurationParser _editorConfigurationParser;

    public TextResourceEditorDataEditor(IDataValueEditorFactory dataValueEditorFactory,
                                        IIOHelper ioHelper,
                                        IEditorConfigurationParser editorConfigurationParser)
        : base(dataValueEditorFactory) {
        _ioHelper = ioHelper;
        _editorConfigurationParser = editorConfigurationParser;
    }

    protected override IConfigurationEditor CreateConfigurationEditor() {
        return new TextResourceEditorConfigurationEditor(_ioHelper, _editorConfigurationParser);
    }
}
