using N3O.Umbraco.Attributes;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Giving.Lookups;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding.Content;

[UmbracoContent(CrowdfundingConstants.FundraiserGoal.Alias)]
public class FundraiserGoalElement : CrowdfundingGoalElement {
    public override string CampaignGoalID => GetValue(x => x.CampaignGoalID);
    
    public override void Content(IPublishedElement content) {
        base.Content(content);
        
        if (Type == AllocationTypes.Fund) {
            Fund = new FundFundraiserGoalElement();
            Fund.Content(content);
        } else if (Type == AllocationTypes.Feedback) {
            Feedback = new FeedbackFundraiserGoalElement();
            Feedback.Content(content);
        } else {
            throw UnrecognisedValueException.For(Type);
        }
    }
    
    public override string FundContentTypeAlias => CrowdfundingConstants.FundraiserGoal.Fund.Alias;
    public override string FeedbackContentTypeAlias => CrowdfundingConstants.FundraiserGoal.Feedback.Alias;
}