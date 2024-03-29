using N3O.Umbraco.Giving.Lookups;
using NodaTime;

namespace N3O.Umbraco.Giving.Models;

public interface IFeedbackCustomField {
    FeedbackCustomFieldType Type { get; }
    string Alias { get; }
    string Name { get; }
    public bool? Bool { get; }
    public LocalDate? Date { get; }
    public string Text { get; }
}