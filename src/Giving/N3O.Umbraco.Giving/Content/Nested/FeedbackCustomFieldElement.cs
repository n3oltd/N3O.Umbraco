using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using Newtonsoft.Json;
using NodaTime;
using System;

namespace N3O.Umbraco.Giving.Content;

public class FeedbackCustomFieldElement : UmbracoElement<FeedbackCustomFieldElement>, IFeedbackCustomField {
    public string Alias => GetValue(x => x.Alias);
    public string DisplayName => GetValue(x => x.DisplayName);
    public string Text => GetValue(x => x.Text);
    public bool? Bool => GetValue(x => x.Bool);
    public DateTime? Date => GetValue(x => x.Date);
    public FeedbackCustomFieldType Type => GetValue(x => x.Type);

    [JsonIgnore]
    string IFeedbackCustomField.Name => DisplayName;
    
    [JsonIgnore]
    LocalDate? IFeedbackCustomField.Date => Date == null ? null : LocalDate.FromDateTime(Date.Value);
}