using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.Cells.DataTypes;

public class CellsConfiguration {
    [ConfigurationField("gridConfiguration",
                        "Grid Configuration",
                        "textarea",
                        Description = "Sets the configuration for the grid")]
    public string GridConfiguration { get; set; }
}
