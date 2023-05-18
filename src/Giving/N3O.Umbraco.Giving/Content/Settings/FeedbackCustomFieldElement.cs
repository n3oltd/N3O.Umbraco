using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Giving.Models;
using NodaTime;

namespace N3O.Umbraco.Giving.Content; 

public class FeedbackCustomFieldElement : UmbracoElement<FeedbackCustomFieldElement>, IFeedbackCustomField {
    [UmbracoProperty("fieldType")]
    public FeedbackCustomFieldType Type => GetValue(x => x.Type);

    public bool? Bool => GetValue(x => x.Bool);
    public LocalDate? Date => GetValue(x => x.Date);
    public string Text => GetValue(x => x.Text);
}