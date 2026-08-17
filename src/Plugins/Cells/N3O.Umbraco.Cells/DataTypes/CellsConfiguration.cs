using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.Cells.DataTypes;

public class CellsConfiguration {
    [ConfigurationField("gridConfiguration")]
    public string GridConfiguration { get; set; }
}
