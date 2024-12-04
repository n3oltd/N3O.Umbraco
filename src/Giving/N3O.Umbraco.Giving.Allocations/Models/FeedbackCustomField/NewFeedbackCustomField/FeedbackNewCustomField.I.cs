using NodaTime;

namespace N3O.Umbraco.Giving.Allocations.Models;

public interface IFeedbackNewCustomField {
    string Alias { get; }
    bool? Bool { get; }
    LocalDate? Date { get; }
    string Text { get; }
}