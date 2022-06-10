using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.Data.DataTypes {
    public class ImportDataEditorConfigurationEditor : ConfigurationEditor<ImportDataEditorConfiguration> {
        public ImportDataEditorConfigurationEditor(IIOHelper ioHelper) : base(ioHelper) { }
    }
}
