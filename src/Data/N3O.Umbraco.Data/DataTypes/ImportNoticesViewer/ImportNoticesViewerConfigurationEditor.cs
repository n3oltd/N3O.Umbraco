using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Data.DataTypes {
    public class ImportNoticesViewerConfigurationEditor : ConfigurationEditor<ImportNoticesViewerConfiguration> {
        public ImportNoticesViewerConfigurationEditor(IIOHelper ioHelper,
                                                      IEditorConfigurationParser editorConfigurationParser)
            : base(ioHelper, editorConfigurationParser) { }
    }
}
