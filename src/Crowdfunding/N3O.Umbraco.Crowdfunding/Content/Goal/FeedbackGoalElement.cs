using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Engage.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Crowdfunding.Content;

[UmbracoContent(CrowdfundingConstants.Goal.Feedback.Alias)]
public class FeedbackGoalElement : UmbracoElement<FeedbackGoalElement>, IFeedbackCrowdfunderGoal {
    public IEnumerable<FeedbackCustomFieldElement> CustomFields => GetNestedAs(x => x.CustomFields);

    public FeedbackScheme GetScheme(ILookups lookups) {
        return GetLookup<FeedbackScheme>(lookups, CrowdfundingConstants.CampaignGoalOption.Feedback.Properties.Scheme);
    }

    IEnumerable<IFeedbackCustomField> IFeedbackCrowdfunderGoal.CustomFields => CustomFields;
}