using N3O.Umbraco.Content;
using N3O.Umbraco.Elements.Lookups;
using N3O.Umbraco.Elements.Models;
using NodaTime;
using System;

namespace N3O.Umbraco.Elements.Content;

public class FeedbackCustomFieldElement : UmbracoElement<FeedbackCustomFieldElement>, IFeedbackCustomField {
    public string Alias => GetValue(x => x.Alias);
    public string DisplayName => GetValue(x => x.DisplayName);
    public string Text => GetValue(x => x.Text);
    public bool? Bool => GetValue(x => x.Bool);
    public DateTime? Date => GetValue(x => x.Date);
    public FeedbackCustomFieldType Type => GetValue(x => x.Type);

    string IFeedbackCustomField.Name => DisplayName;
    
    LocalDate? IFeedbackCustomField.Date => Date == null ? null : LocalDate.FromDateTime(Date.Value);

    public FeedbackCustomField ToFeedbackCustomField() {
        return new FeedbackCustomField(this);
    }
}