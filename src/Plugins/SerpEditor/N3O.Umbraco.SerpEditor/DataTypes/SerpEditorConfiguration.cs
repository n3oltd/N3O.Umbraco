using Umbraco.Cms.Core.PropertyEditors;

namespace N3O.Umbraco.SerpEditor.DataTypes;

public class SerpEditorConfiguration {
    [ConfigurationField("maxCharsDescription")]
    public string MaxCharsDescription { get; set; }

    [ConfigurationField("maxCharsTitle")]
    public string MaxCharsTitle { get; set; }
}
