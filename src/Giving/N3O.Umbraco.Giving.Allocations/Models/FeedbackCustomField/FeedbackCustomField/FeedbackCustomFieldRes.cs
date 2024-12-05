using N3O.Umbraco.Giving.Allocations.Lookups;
using NodaTime;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class FeedbackCustomFieldRes : IFeedbackCustomField {
    public FeedbackCustomFieldType Type { get; set; }
    public string Alias { get; set; }
    public string Name { get; set; }
    public bool? Bool { get; set; }
    public LocalDate? Date { get; set; }
    public string Text { get; set; }
}