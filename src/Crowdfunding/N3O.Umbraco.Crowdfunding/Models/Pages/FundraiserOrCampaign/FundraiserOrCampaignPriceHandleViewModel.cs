using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Content;

namespace N3O.Umbraco.CrowdFunding.Models;

public class FundraiserOrCampaignPriceHandleViewModel {
    public Money Amount { get; set; }
    public string Description { get; set; }

    public static FundraiserOrCampaignPriceHandleViewModel For(ICrowdfundingHelper crowdfundingHelper,
                                                               PriceHandleElement fundraiserPriceHandle) {
        var priceHandle = new FundraiserOrCampaignPriceHandleViewModel();

        priceHandle.Amount = crowdfundingHelper.GetQuoteMoney(fundraiserPriceHandle.Amount);
        priceHandle.Description = fundraiserPriceHandle.Description;

        return priceHandle;
    }
}