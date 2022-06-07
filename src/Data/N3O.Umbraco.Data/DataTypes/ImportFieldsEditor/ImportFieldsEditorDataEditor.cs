using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.Data.DataTypes {
    [DataEditor(DataConstants.PropertyEditorAliases.ImportFieldsEditor,
                DataEditorName,
                "~/App_Plugins/N3O.Umbraco.Data.ImportFieldsEditor/N3O.Umbraco.Data.ImportFieldsEditor.html",
                HideLabel = false,
                ValueType = "JSON")]
    public class ImportFieldsEditorDataEditor : DataEditor {
        public const string DataEditorName = "N3O Import Fields Editor";
        
        private readonly IIOHelper _ioHelper;
    
        public ImportFieldsEditorDataEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper)
            : base(dataValueEditorFactory) {
            _ioHelper = ioHelper;
        }

        protected override IConfigurationEditor CreateConfigurationEditor() {
            return new ImportFieldsEditorConfigurationEditor(_ioHelper);
        }
    }
}
