using N3O.Umbraco.Elements.Lookups;
using NodaTime;

namespace N3O.Umbraco.Elements.Models;

public class FeedbackCustomFieldRes : IFeedbackCustomField {
    public FeedbackCustomFieldType Type { get; set; }
    public string Alias { get; set; }
    public string Name { get; set; }
    public bool? Bool { get; set; }
    public LocalDate? Date { get; set; }
    public string Text { get; set; }
}