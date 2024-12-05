using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models;

public class PricingDataMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IPricing, PricingData>((_, _) => new PricingData(), Map);
    }

    // Umbraco.Code.MapAll -Locked -Amount -CurrencyValues
    private void Map(IPricing src, PricingData dest, MapperContext ctx) {
         ctx.Map<IPrice, PriceData>(src, dest);
         dest.PriceRules = src.Rules.OrEmpty().Select(ctx.Map<IPricingRule, PricingRuleData>).ToList();
    }
}
