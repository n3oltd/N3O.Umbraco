using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Content;

public abstract class CrowdfundingFeedbackGoalElement : UmbracoElement<CrowdfundingFeedbackGoalElement> {
    public FeedbackScheme Scheme => GetValue(x => x.Scheme);
    public IEnumerable<CrowdfundingFeedbackCustomFieldElement> CustomFields => GetNestedAs(x => x.CustomFields);
}