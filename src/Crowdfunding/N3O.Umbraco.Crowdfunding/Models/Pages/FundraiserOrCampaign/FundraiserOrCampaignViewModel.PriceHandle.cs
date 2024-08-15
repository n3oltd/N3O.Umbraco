using N3O.Umbraco.Crowdfunding;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Content;

namespace N3O.Umbraco.CrowdFunding.Models;

public partial class FundraiserOrCampaignViewModel<TContent> {
    public class PriceHandle {
        public Money Amount { get; set; }
        public string Description { get; set; }

        public static PriceHandle For(ICrowdfundingHelper crowdfundingHelper,
                                      PriceHandleElement fundraiserPriceHandle) {
            var priceHandle = new PriceHandle();
            
            priceHandle.Amount = crowdfundingHelper.GetQuoteMoney(fundraiserPriceHandle.Amount);
            priceHandle.Description = fundraiserPriceHandle.Description;

            return priceHandle;
        }
    }
}