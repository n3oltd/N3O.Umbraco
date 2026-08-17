using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.EditorJs.DataTypes;

public class EditorJsConfigurationEditor : ConfigurationEditor<EditorJsConfiguration> {
    public EditorJsConfigurationEditor(IIOHelper ioHelper)
        : base(ioHelper) { }
}
