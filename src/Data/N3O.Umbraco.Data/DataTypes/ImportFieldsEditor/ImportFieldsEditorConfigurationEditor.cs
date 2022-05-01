using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.Data.DataTypes {
    public class ImportFieldsEditorConfigurationEditor : ConfigurationEditor<ImportFieldsEditorConfiguration> {
        public ImportFieldsEditorConfigurationEditor(IIOHelper ioHelper) : base(ioHelper) { }
    }
}
