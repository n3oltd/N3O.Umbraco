using N3O.Umbraco.Financial;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Pricing.Models {
    public class PriceMapping : IMapDefinition {
        private readonly IPriceCalculator _priceCalculator;

        public PriceMapping(IPriceCalculator priceCalculator) {
            _priceCalculator = priceCalculator;
        }
        
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<IPrice, PriceRes>((_, _) => new PriceRes(), Map);
        }

        // Umbraco.Code.MapAll
        private void Map(IPrice src, PriceRes dest, MapperContext ctx) {
            dest.Value = ctx.Map<Money, MoneyRes>(_priceCalculator.InCurrentCurrency(src));
            dest.Locked = src.Locked;
        }
    }
}