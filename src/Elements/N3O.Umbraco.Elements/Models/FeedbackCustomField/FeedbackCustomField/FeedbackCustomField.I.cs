using N3O.Umbraco.Elements.Lookups;
using NodaTime;

namespace N3O.Umbraco.Elements.Models;

public interface IFeedbackCustomField {
    FeedbackCustomFieldType Type { get; }
    string Alias { get; }
    string Name { get; }
    bool? Bool { get; }
    LocalDate? Date { get; }
    string Text { get; }
}