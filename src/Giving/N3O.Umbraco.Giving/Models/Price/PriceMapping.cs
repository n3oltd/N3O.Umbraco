using N3O.Umbraco.Forex.Models;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Models {
    public class PriceMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<IPrice, PriceRes>((_, _) => new PriceRes(), Map);
        }

        // Umbraco.Code.MapAll
        private void Map(IPrice src, PriceRes dest, MapperContext ctx) {
            dest.Amount = src.Amount;
            dest.CurrencyValues = ctx.Map<decimal, CurrencyValuesRes>(src.Amount);
            dest.Locked = src.Locked;
        }
    }
}