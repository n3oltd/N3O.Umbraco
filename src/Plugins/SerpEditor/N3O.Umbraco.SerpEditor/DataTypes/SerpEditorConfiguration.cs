using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.SerpEditor.DataTypes;

public class SerpEditorConfiguration {
    [ConfigurationField("maxCharsDescription")]
    public int? MaxCharsDescription { get; set; }

    [ConfigurationField("maxCharsTitle")]
    public int? MaxCharsTitle { get; set; }
}
