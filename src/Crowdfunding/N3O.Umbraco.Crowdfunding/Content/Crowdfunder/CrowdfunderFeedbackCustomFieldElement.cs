using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Lookups;
using System;

namespace N3O.Umbraco.Crowdfunding.Content;

public class CrowdfunderFeedbackCustomFieldElement : UmbracoElement<CrowdfunderFeedbackCustomFieldElement> {
    public string Alias => GetValue(x => x.Alias);
    public string Text => GetValue(x => x.Text);
    public bool? Bool => GetValue(x => x.Bool);
    public DateTime? Date => GetValue(x => x.Date);
    public FeedbackCustomFieldType Type => GetValue(x => x.Type);
}