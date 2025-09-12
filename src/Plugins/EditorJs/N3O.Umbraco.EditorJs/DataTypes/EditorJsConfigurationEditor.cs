using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.EditorJs.DataTypes;

public class EditorJsConfigurationEditor : ConfigurationEditor<EditorJsConfiguration> {
    public EditorJsConfigurationEditor(IIOHelper ioHelper, IEditorConfigurationParser editorConfigurationParser)
        : base(ioHelper, editorConfigurationParser) { }
}
