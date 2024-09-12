using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Content;

namespace N3O.Umbraco.CrowdFunding.Models;

public class FundraiserOrCampaignPriceHandleViewModel {
    public Money Amount { get; private set; }
    public string Description { get; private set; }

    public static FundraiserOrCampaignPriceHandleViewModel For(ICrowdfundingHelper crowdfundingHelper,
                                                               PriceHandleElement fundraiserPriceHandle) {
        var viewModel = new FundraiserOrCampaignPriceHandleViewModel();

        viewModel.Amount = crowdfundingHelper.GetQuoteMoney(fundraiserPriceHandle.Amount);
        viewModel.Description = fundraiserPriceHandle.Description;

        return viewModel;
    }
}