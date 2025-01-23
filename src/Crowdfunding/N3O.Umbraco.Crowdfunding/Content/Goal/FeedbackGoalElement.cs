using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crm.Models;
using N3O.Umbraco.Giving.Allocations.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Content;

[UmbracoContent(CrowdfundingConstants.Goal.Feedback.Alias)]
public class FeedbackGoalElement : UmbracoElement<FeedbackGoalElement>, IFeedbackCrowdfunderGoal {
    public FeedbackScheme Scheme => GetValue(x => x.Scheme);
    public IEnumerable<FeedbackCustomFieldElement> CustomFields => GetNestedAs(x => x.CustomFields);

    IEnumerable<IFeedbackCustomField> IFeedbackCrowdfunderGoal.CustomFields => CustomFields;
}