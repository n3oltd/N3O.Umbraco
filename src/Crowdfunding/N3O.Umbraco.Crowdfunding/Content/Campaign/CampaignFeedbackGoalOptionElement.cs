using N3O.Umbraco.Content;
using N3O.Umbraco.Giving.Lookups;

namespace N3O.Umbraco.Crowdfunding.Content;

public class CampaignFeedbackGoalOptionElement : UmbracoElement<CampaignFeedbackGoalOptionElement> {
    public FeedbackScheme Scheme => GetValue(x => x.Scheme);
}