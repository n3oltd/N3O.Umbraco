using N3O.Umbraco.Extensions;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class PricingMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IPricing, PricingRes>((_, _) => new PricingRes(), Map);
    }

    // Umbraco.Code.MapAll -Locked -Amount -CurrencyValues
    private void Map(IPricing src, PricingRes dest, MapperContext ctx) {
         dest.Price = src.Price.IfNotNull(ctx.Map<IPrice, PriceRes>);
         dest.PriceRules = src.Rules.OrEmpty().Select(ctx.Map<IPricingRule, PricingRuleRes>).ToList();
    }
}
