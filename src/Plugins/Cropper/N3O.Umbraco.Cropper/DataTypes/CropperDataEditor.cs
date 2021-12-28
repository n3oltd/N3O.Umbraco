using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.Cropper.DataTypes {
    [DataEditor(CropperConstants.PropertyEditorAlias,
                "N3O Cropper",
                "~/App_Plugins/N3O.Umbraco.Cropper/N3O.Umbraco.Cropper.html",
                HideLabel = false,
                ValueType = "JSON")]
    public class CropperDataEditor : DataEditor {
        private readonly IIOHelper _ioHelper;
    
        public CropperDataEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper)
            : base(dataValueEditorFactory) {
            _ioHelper = ioHelper;
        }

        protected override IConfigurationEditor CreateConfigurationEditor() {
            return new CropperConfigurationEditor(_ioHelper);
        }
    }
}
