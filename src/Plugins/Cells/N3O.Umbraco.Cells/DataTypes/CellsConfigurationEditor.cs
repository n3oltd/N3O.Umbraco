using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.Cells.DataTypes;

public class CellsConfigurationEditor : ConfigurationEditor<CellsConfiguration> {
    public CellsConfigurationEditor(IIOHelper ioHelper)
        : base(ioHelper) { }
}
