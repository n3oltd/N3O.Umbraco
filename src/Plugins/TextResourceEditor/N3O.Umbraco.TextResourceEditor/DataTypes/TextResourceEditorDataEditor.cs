using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.TextResourceEditor.DataTypes {
    [DataEditor(TextResourceEditorConstants.PropertyEditorAlias,
                "N3O Text Resource Editor",
                "~/App_Plugins/N3O.Umbraco.TextResourceEditor/N3O.Umbraco.TextResourceEditor.html",
                HideLabel = false,
                ValueType = "JSON")]
    public class TextResourceEditorDataEditor : DataEditor {
        private readonly IIOHelper _ioHelper;
    
        public TextResourceEditorDataEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper)
            : base(dataValueEditorFactory) {
            _ioHelper = ioHelper;
        }

        protected override IConfigurationEditor CreateConfigurationEditor() {
            return new TextResourceEditorConfigurationEditor(_ioHelper);
        }
    }
}
