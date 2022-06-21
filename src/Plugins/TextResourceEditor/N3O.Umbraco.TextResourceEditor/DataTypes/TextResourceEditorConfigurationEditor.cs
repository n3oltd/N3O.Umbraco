using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.TextResourceEditor.DataTypes {
    public class TextResourceEditorConfigurationEditor : ConfigurationEditor<TextResourceEditorConfiguration> {
        public TextResourceEditorConfigurationEditor(IIOHelper ioHelper,
                                                     IEditorConfigurationParser editorConfigurationParser)
            : base(ioHelper, editorConfigurationParser) { }
    }
}
