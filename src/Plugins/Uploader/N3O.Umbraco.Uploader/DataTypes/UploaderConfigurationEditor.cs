using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.Uploader.DataTypes {
    public class UploaderConfigurationEditor : ConfigurationEditor<UploaderConfiguration> {
        public UploaderConfigurationEditor(IIOHelper ioHelper) : base(ioHelper) { }
    }
}
