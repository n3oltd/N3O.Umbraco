using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Elements.Lookups;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NodaTime;

namespace N3O.Umbraco.Elements.Models;

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

    public JValue GetJValue() {
        var jValue = JValue.CreateNull();
        
        if (Type == FeedbackCustomFieldTypes.Bool) {
            if (Bool.HasValue()) {
                jValue = new JValue(Bool.GetValueOrThrow());
            }
        } else if (Type == FeedbackCustomFieldTypes.Date) {
            if (Date.HasValue()) {
                jValue = new JValue(Date.GetValueOrThrow().ToDateTimeUnspecified());
            }
        } else if (Type == FeedbackCustomFieldTypes.Text) {
            if (Text.HasValue()) {
                jValue = new JValue(Text);
            }
        } else {
            throw UnrecognisedValueException.For(Type);
        }

        return jValue;
    }
}