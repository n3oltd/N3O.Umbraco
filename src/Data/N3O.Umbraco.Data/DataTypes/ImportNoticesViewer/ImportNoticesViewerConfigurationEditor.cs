using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.Data.DataTypes {
    public class ImportNoticesViewerConfigurationEditor : ConfigurationEditor<ImportNoticesViewerConfiguration> {
        public ImportNoticesViewerConfigurationEditor(IIOHelper ioHelper) : base(ioHelper) { }
    }
}
