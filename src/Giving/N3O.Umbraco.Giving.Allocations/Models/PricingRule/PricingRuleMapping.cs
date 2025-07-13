using Umbraco.Cms.Core.Mapping;
using Umbraco.Extensions;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class PricingRuleMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IPricingRule, PricingRuleRes>((_, _) => new PricingRuleRes(), Map);
    }

    // Umbraco.Code.MapAll -Locked -Amount -CurrencyValues
    private void Map(IPricingRule src, PricingRuleRes dest, MapperContext ctx) {
         dest.Price = src.Price.IfNotNull(ctx.Map<IPrice, PriceRes>);
         dest.FundDimensions = ctx.Map<IFundDimensionValues, FundDimensionValuesRes>(src.FundDimensions);
    }
}
