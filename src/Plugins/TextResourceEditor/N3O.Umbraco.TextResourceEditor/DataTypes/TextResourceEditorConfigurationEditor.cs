using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.TextResourceEditor.DataTypes {
    public class TextResourceEditorConfigurationEditor : ConfigurationEditor<TextResourceEditorConfiguration> {
        public TextResourceEditorConfigurationEditor(IIOHelper ioHelper) : base(ioHelper) { }
    }
}
