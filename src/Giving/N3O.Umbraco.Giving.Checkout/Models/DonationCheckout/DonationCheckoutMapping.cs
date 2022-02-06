using N3O.Umbraco.Extensions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Models;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Checkout.Models {
    public class DonationCheckoutMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<DonationCheckout, DonationCheckoutRes>((_, _) => new DonationCheckoutRes(), Map);
        }

        // Umbraco.Code.MapAll
        private void Map(DonationCheckout src, DonationCheckoutRes dest, MapperContext ctx) {
            dest.Allocations = src.Allocations
                                  .OrEmpty()
                                  .Select(ctx.Map<Allocation, AllocationRes>)
                                  .ToList();
            dest.Payment = src.Payment;
            dest.Total = ctx.Map<Money, MoneyRes>(src.Total);
            dest.IsComplete = src.IsComplete;
            dest.IsRequired = src.IsRequired;
        }
    }
}