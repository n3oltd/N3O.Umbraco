namespace N3O.Umbraco.Elements.Models;

public class EntryData {
    public string TypeId { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public CategoryEntryData Category { get; set; }
    public OptionEntryData Option { get; set; }
}