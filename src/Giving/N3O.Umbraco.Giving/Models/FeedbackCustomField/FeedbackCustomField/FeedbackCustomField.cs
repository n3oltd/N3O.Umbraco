using N3O.Umbraco.Giving.Lookups;
using Newtonsoft.Json;
using NodaTime;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackCustomField : Value, IFeedbackCustomField {
    [JsonConstructor]
    public FeedbackCustomField(FeedbackCustomFieldType type,
                               string alias,
                               string name,
                               bool? @bool,
                               LocalDate? date,
                               string text) {
        Type = type;
        Alias = alias;
        Name = name;
        Bool = @bool;
        Date = date;
        Text = text;
    }

    public FeedbackCustomField(IFeedbackCustomField customField)
        : this(customField.Type,
               customField.Alias,
               customField.Name,
               customField.Bool,
               customField.Date,
               customField.Text) { }

    public FeedbackCustomFieldType Type { get; }
    public string Alias { get; }
    public string Name { get; }
    public bool? Bool { get; }
    public LocalDate? Date { get; }
    public string Text { get; }
}