using N3O.Umbraco.Giving.Allocations.Models;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models;

public class PricingRuleDataMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IPricingRule, PricingRuleData>((_, _) => new PricingRuleData(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(IPricingRule src, PricingRuleData dest, MapperContext ctx) {
         dest.FundDimensions = ctx.Map<IFundDimensionValues, FundDimensionValuesData>(src.FundDimensions);
         dest.Amount = src.Amount;
         dest.Locked = src.Locked;
    }
}
