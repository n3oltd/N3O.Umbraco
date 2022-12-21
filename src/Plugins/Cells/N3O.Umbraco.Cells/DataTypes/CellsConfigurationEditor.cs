using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cells.DataTypes;

public class CellsConfigurationEditor : ConfigurationEditor<CellsConfiguration> {
    public CellsConfigurationEditor(IIOHelper ioHelper, IEditorConfigurationParser editorConfigurationParser)
        : base(ioHelper, editorConfigurationParser) { }
}
