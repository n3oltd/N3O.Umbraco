using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.SerpEditor.DataTypes {
    public class SerpEditorConfigurationEditor : ConfigurationEditor<SerpEditorConfiguration> {
        public SerpEditorConfigurationEditor(IIOHelper ioHelper, IEditorConfigurationParser editorConfigurationParser)
            : base(ioHelper, editorConfigurationParser) { }
    }
}
