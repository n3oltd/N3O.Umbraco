using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Uploader.DataTypes {
    public class UploaderConfigurationEditor : ConfigurationEditor<UploaderConfiguration> {
        public UploaderConfigurationEditor(IIOHelper ioHelper, IEditorConfigurationParser editorConfigurationParser)
            : base(ioHelper, editorConfigurationParser) { }
    }
}
