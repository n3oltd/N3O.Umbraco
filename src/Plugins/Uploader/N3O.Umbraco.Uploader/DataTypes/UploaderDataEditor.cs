using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.Uploader.DataTypes {
    [DataEditor(UploaderConstants.PropertyEditorAlias,
                "N3O Uploader",
                "~/App_Plugins/N3O.Umbraco.Uploader/N3O.Umbraco.Uploader.html",
                HideLabel = false,
                ValueType = "JSON")]
    public class UploaderDataEditor : DataEditor {
        private readonly IIOHelper _ioHelper;
    
        public UploaderDataEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper)
            : base(dataValueEditorFactory) {
            _ioHelper = ioHelper;
        }

        protected override IConfigurationEditor CreateConfigurationEditor() {
            return new UploaderConfigurationEditor(_ioHelper);
        }
    }
}
