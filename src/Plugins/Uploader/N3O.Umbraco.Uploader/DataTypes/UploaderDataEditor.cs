using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Uploader.DataTypes {
    [DataEditor(UploaderConstants.PropertyEditorAlias,
                "N3O Uploader",
                "~/App_Plugins/N3O.Umbraco.Uploader/N3O.Umbraco.Uploader.html",
                HideLabel = false,
                ValueType = "JSON")]
    public class UploaderDataEditor : DataEditor {
        private readonly IIOHelper _ioHelper;
        private readonly IEditorConfigurationParser _editorConfigurationParser;

        public UploaderDataEditor(IDataValueEditorFactory dataValueEditorFactory,
                                  IIOHelper ioHelper,
                                  IEditorConfigurationParser editorConfigurationParser)
            : base(dataValueEditorFactory) {
            _ioHelper = ioHelper;
            _editorConfigurationParser = editorConfigurationParser;
        }

        protected override IConfigurationEditor CreateConfigurationEditor() {
            return new UploaderConfigurationEditor(_ioHelper, _editorConfigurationParser);
        }
    }
}
