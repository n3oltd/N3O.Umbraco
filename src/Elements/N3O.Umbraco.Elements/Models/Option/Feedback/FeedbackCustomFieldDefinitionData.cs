namespace N3O.Umbraco.Elements.Models;

public class FeedbackCustomFieldDefinitionData {
    public string TypeId { get; set; }
    public string Alias { get; set; }
    public string Name { get; set; }
    public bool Required { get; set; }
    public int? TextMaxLength { get; set; }
}