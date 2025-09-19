using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Extensions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Crowdfunding.Content;

public class FundGoalElementValidator : GoalElementValidator<FundGoalElement> {
    private readonly ILookups _lookups;
    
    public FundGoalElementValidator(IContentHelper contentHelper, ILookups lookups) : base(contentHelper, lookups) {
        _lookups = lookups;
    }

    protected override IFundDimensionOptions GetFundDimensionOptions(ContentProperties content) {
        return GetDonationItem(content).FundDimensionOptions;
    }
    
    protected override void ValidatePriceLocked(ContentProperties content) {
        var property = content.GetPropertyByAlias(CrowdfundingConstants.Goal.Fund.Properties.DonationItem);
        
        var donationItem = property.IfNotNull(x => ContentHelper.GetLookupValue<DonationItem>(_lookups, x));
        
        if (donationItem.HasLockedPrice()) {
            ErrorResult($"The donation item {donationItem.Name} has a locked price which is not permitted");
        }
    }

    private DonationItem GetDonationItem(ContentProperties content) {
        var donationItem = content.GetPropertyByAlias(CrowdfundingConstants.Goal.Fund.Properties.DonationItem)
                                  .IfNotNull(x => ContentHelper.GetLookupValue<DonationItem>(_lookups, x));

        return donationItem;
    }
}