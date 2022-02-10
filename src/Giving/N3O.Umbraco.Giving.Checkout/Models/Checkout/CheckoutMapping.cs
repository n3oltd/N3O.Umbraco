using N3O.Umbraco.Accounts.Models;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Checkout.Models {
    public class CheckoutMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<Entities.Checkout, CheckoutRes>((_, _) => new CheckoutRes(), Map);
        }

        // Umbraco.Code.MapAll
        private void Map(Entities.Checkout src, CheckoutRes dest, MapperContext ctx) {
            dest.RevisionId = src.RevisionId;
            dest.Reference = src.Reference;
            dest.Currency = src.Currency;
            dest.Progress = ctx.Map<CheckoutProgress, CheckoutProgressRes>(src.Progress);
            dest.Account = ctx.Map<Account, AccountRes>(src.Account);
            dest.Donation = ctx.Map<DonationCheckout, DonationCheckoutRes>(src.Donation);
            dest.RegularGiving = ctx.Map<RegularGivingCheckout, RegularGivingCheckoutRes>(src.RegularGiving);
            dest.IsComplete = src.IsComplete;
        }
    }
}