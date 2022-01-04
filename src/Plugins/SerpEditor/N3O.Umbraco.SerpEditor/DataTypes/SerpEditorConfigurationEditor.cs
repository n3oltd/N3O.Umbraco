using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.SerpEditor.DataTypes {
    public class SerpEditorConfigurationEditor : ConfigurationEditor<SerpEditorConfiguration> {
        public SerpEditorConfigurationEditor(IIOHelper ioHelper) : base(ioHelper) { }
    }
}
