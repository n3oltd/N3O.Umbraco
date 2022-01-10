using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.SerpEditor.DataTypes {
    [DataEditor(SerpEditorConstants.PropertyEditorAlias,
                "N3O SERP Editor",
                "~/App_Plugins/N3O.Umbraco.SerpEditor/N3O.Umbraco.SerpEditor.html",
                HideLabel = false,
                ValueType = "JSON")]
    public class SerpEditorDataEditor : DataEditor {
        private readonly IIOHelper _ioHelper;
    
        public SerpEditorDataEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper)
            : base(dataValueEditorFactory) {
            _ioHelper = ioHelper;
        }

        protected override IConfigurationEditor CreateConfigurationEditor() {
            return new SerpEditorConfigurationEditor(_ioHelper);
        }
    }
}
