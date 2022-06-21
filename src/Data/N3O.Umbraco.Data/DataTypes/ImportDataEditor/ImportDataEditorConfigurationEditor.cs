using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Data.DataTypes;

public class ImportDataEditorConfigurationEditor : ConfigurationEditor<ImportDataEditorConfiguration> {
    public ImportDataEditorConfigurationEditor(IIOHelper ioHelper,
                                               IEditorConfigurationParser editorConfigurationParser)
        : base(ioHelper, editorConfigurationParser) { }
}
