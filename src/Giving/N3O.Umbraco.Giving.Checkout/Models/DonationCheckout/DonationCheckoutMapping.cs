using N3O.Umbraco.Financial;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Checkout.Models {
    public class DonationCheckoutMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<DonationCheckout, DonationCheckoutRes>((_, _) => new DonationCheckoutRes(), Map);
        }

        // Umbraco.Code.MapAll
        private void Map(DonationCheckout src, DonationCheckoutRes dest, MapperContext ctx) {
            dest.Allocations = src.Allocations;
            dest.Total = ctx.Map<Money, MoneyRes>(src.Total);
        }
    }
}