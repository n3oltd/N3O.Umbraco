using N3O.Umbraco.Giving.Lookups;
using Newtonsoft.Json;
using NodaTime;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackCustomField : IFeedbackCustomField {
    [JsonConstructor]
    public FeedbackCustomField(FeedbackCustomFieldType type, bool? @bool, LocalDate? date, string text) {
        Type = type;
        Bool = @bool;
        Date = date;
        Text = text;
    }

    public FeedbackCustomField(IFeedbackCustomField customField)
        : this(customField.Type, customField.Bool, customField.Date, customField.Text) { }
    
    public FeedbackCustomFieldType Type { get; }
    public bool? Bool { get; }
    public LocalDate? Date { get; }
    public string Text { get; }
}