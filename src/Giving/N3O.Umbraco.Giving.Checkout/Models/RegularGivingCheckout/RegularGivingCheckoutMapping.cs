using N3O.Umbraco.Financial;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Checkout.Models {
    public class RegularGivingCheckoutMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<RegularGivingCheckout, RegularGivingCheckoutRes>((_, _) => new RegularGivingCheckoutRes(), Map);
        }

        // Umbraco.Code.MapAll
        private void Map(RegularGivingCheckout src, RegularGivingCheckoutRes dest, MapperContext ctx) {
            dest.Allocations = src.Allocations;
            dest.Total = ctx.Map<Money, MoneyRes>(src.Total);
            dest.CollectionDay = src.CollectionDay;
            dest.FirstCollectionDate = src.FirstCollectionDate;
        }
    }
}