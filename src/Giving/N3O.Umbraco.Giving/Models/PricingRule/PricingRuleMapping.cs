using N3O.Giving.Models;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Models {
    public class PricingRuleMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<IPricingRule, PricingRuleRes>((_, _) => new PricingRuleRes(), Map);
        }

        // Umbraco.Code.MapAll -Locked -Amount
        private void Map(IPricingRule src, PricingRuleRes dest, MapperContext ctx) {
             ctx.Map<IPrice, PriceRes>(src, dest);
             dest.FundDimensions = ctx.Map<IFundDimensionValues, FundDimensionValuesRes>(src.FundDimensions);
        }
    }
}