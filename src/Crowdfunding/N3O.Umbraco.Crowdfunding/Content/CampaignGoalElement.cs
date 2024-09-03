using N3O.Umbraco.Attributes;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Giving.Lookups;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding.Content;

[UmbracoContent(CrowdfundingConstants.CampaignGoal.Alias)]
public class CampaignGoalElement : CrowdfundingGoalElement {
    public override void Content(IPublishedElement content) {
        base.Content(content);
        
        if (Type == AllocationTypes.Fund) {
            Fund = new FundCampaignGoalElement();
            Fund.Content(content);
        } else if (Type == AllocationTypes.Feedback) {
            Feedback = new FeedbackCampaignGoalElement();
            Feedback.Content(content);
        } else {
            throw UnrecognisedValueException.For(Type);
        }
    }

    public override string FundContentTypeAlias => CrowdfundingConstants.CampaignGoal.Fund.Alias;
    public override string FeedbackContentTypeAlias => CrowdfundingConstants.CampaignGoal.Feedback.Alias;
}