using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding.Content;

public class CampaignFeedbackGoalOptionElementValidator : CampaignGoalOptionElementValidator<CampaignFeedbackGoalOptionElement> {
    public CampaignFeedbackGoalOptionElementValidator(IContentHelper contentHelper) : base(contentHelper) { }

    protected override IFundDimensionsOptions GetFundDimensionOptions(ContentProperties content) {
        return GetFeedbackScheme(content);
    }
    
    protected override void ValidatePriceLocked(ContentProperties content) {
        var property = content.GetPropertyByAlias(CrowdfundingConstants.Goal.Feedback.Properties.Scheme);
        
        var feedbackScheme = property.IfNotNull(x => ContentHelper.GetMultiNodeTreePickerValue<IPublishedContent>(x)
                                                                  .As<FeedbackScheme>());
        
        if (feedbackScheme.Price.Locked) {
            ErrorResult($"{feedbackScheme.Name} Feedback has locked price which is not permitted");
        }
    }
    
    private FeedbackScheme GetFeedbackScheme(ContentProperties content) {
        var feedbackScheme = content.GetPropertyByAlias(CrowdfundingConstants.Goal.Feedback.Properties.Scheme)
                                    .IfNotNull(x => ContentHelper.GetMultiNodeTreePickerValue<IPublishedContent>(x)
                                                                 .As<FeedbackScheme>());

        return feedbackScheme;
    }
}