using N3O.Umbraco.Attributes;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Giving.Lookups;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding.Content;

[UmbracoContent(CrowdfundingConstants.FundraiserGoal.Alias)]
public class FundraiserGoalElement : CrowdfundingGoalElement {
    public override string CampaignGoalId => GetValue(x => x.CampaignGoalId);
    
    public override void Content(IPublishedElement content, IPublishedContent parent) {
        base.Content(content, parent);
        
        if (Type == AllocationTypes.Fund) {
            Fund = new FundFundraiserGoalElement();
            Fund.Content(content, parent);
        } else if (Type == AllocationTypes.Feedback) {
            Feedback = new FeedbackFundraiserGoalElement();
            Feedback.Content(content, parent);
        } else {
            throw UnrecognisedValueException.For(Type);
        }
    }
    
    public override string FundContentTypeAlias => CrowdfundingConstants.FundraiserGoal.Fund.Alias;
    public override string FeedbackContentTypeAlias => CrowdfundingConstants.FundraiserGoal.Feedback.Alias;
}