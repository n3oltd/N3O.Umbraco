using N3O.Umbraco.Giving.Lookups;
using NodaTime;

namespace N3O.Umbraco.Giving.Models;

public interface IFeedbackCustomField {
    FeedbackCustomFieldType Type { get; }
    string Alias { get; }
    string Name { get; }
    bool? Bool { get; }
    LocalDate? Date { get; }
    string Text { get; }
}