using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding.Content;

public class CampaignFundGoalOptionValidator : CampaignGoalOptionElementValidator<CampaignFundGoalOptionElement> {
    public CampaignFundGoalOptionValidator(IContentHelper contentHelper) : base(contentHelper) { }

    protected override IFundDimensionsOptions GetFundDimensionOptions(ContentProperties content) {
        return GetDonationItem(content);
    }

    protected override void ValidatePriceLocked(ContentProperties content) {
        var property = content.GetPropertyByAlias(CrowdfundingConstants.Goal.Fund.Properties.DonationItem);
        
        var donationItem = property.IfNotNull(x => ContentHelper.GetMultiNodeTreePickerValue<IPublishedContent>(x)
                                                                .As<DonationItem>());
        
        if (donationItem.Price.Locked) {
            ErrorResult($"The donation item {donationItem.Name} has a locked price which is not permitted");
        }
    }

    private DonationItem GetDonationItem(ContentProperties content) {
        var donationItem = content.GetPropertyByAlias(CrowdfundingConstants.Goal.Fund.Properties.DonationItem)
                                  .IfNotNull(x => ContentHelper.GetMultiNodeTreePickerValue<IPublishedContent>(x)
                                                               .As<DonationItem>());

        return donationItem;
    }
}