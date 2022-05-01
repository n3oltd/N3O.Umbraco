using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.Data.DataTypes {
    public class ImportErrorsViewerConfigurationEditor : ConfigurationEditor<ImportErrorsViewerConfiguration> {
        public ImportErrorsViewerConfigurationEditor(IIOHelper ioHelper) : base(ioHelper) { }
    }
}
