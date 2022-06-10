using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.Data.DataTypes {
    [DataEditor(DataConstants.PropertyEditorAliases.ImportNoticesViewer,
                DataEditorName,
                "~/App_Plugins/N3O.Umbraco.Data.ImportNoticesViewer/N3O.Umbraco.Data.ImportNoticesViewer.html",
                HideLabel = false,
                ValueType = "JSON")]
    public class ImportNoticesViewerDataEditor : DataEditor {
        public const string DataEditorName = "N3O Import Notices Viewer";
        
        private readonly IIOHelper _ioHelper;
    
        public ImportNoticesViewerDataEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper)
            : base(dataValueEditorFactory) {
            _ioHelper = ioHelper;
        }

        protected override IConfigurationEditor CreateConfigurationEditor() {
            return new ImportNoticesViewerConfigurationEditor(_ioHelper);
        }
    }
}
