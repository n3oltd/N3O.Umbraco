using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.Data.DataTypes {
    [DataEditor(DataConstants.PropertyEditorAliases.ImportDataEditor,
                DataEditorName,
                "~/App_Plugins/N3O.Umbraco.Data.ImportDataEditor/N3O.Umbraco.Data.ImportDataEditor.html",
                HideLabel = false,
                ValueType = "JSON")]
    public class ImportDataEditorDataEditor : DataEditor {
        public const string DataEditorName = "N3O Import Data Editor";
        
        private readonly IIOHelper _ioHelper;
    
        public ImportDataEditorDataEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper)
            : base(dataValueEditorFactory) {
            _ioHelper = ioHelper;
        }

        protected override IConfigurationEditor CreateConfigurationEditor() {
            return new ImportDataEditorConfigurationEditor(_ioHelper);
        }
    }
}
