﻿using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Allocations.Lookups;

namespace N3O.Umbraco.Crowdfunding.Content;

[UmbracoContent(CrowdfundingConstants.CampaignGoalOption.Feedback.Alias)]
public class CampaignFeedbackGoalOptionElement : UmbracoElement<CampaignFeedbackGoalOptionElement> {
    public FeedbackScheme Scheme => GetValue(x => x.Scheme);
}