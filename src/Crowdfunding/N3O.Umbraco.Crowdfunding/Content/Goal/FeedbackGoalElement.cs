using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Content;
using N3O.Umbraco.Giving.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Content;

public class FeedbackGoalElement : UmbracoElement<FeedbackGoalElement> {
    public FeedbackScheme Scheme => GetValue(x => x.Scheme);
    public IEnumerable<FeedbackCustomFieldElement> CustomFields => GetNestedAs(x => x.CustomFields);
}