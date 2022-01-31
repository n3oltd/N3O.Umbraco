using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Pricing.Models {
    public class PricingRuleMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<IPricingRule, PricingRuleRes>((_, _) => new PricingRuleRes(), Map);
        }

        // Umbraco.Code.MapAll -Locked -Value
        private void Map(IPricingRule src, PricingRuleRes dest, MapperContext ctx) {
             ctx.Map<IPrice, PriceRes>(src, dest);
             dest.Dimension1Options = src.Dimension1Options;
             dest.Dimension2Options = src.Dimension2Options;
             dest.Dimension3Options = src.Dimension3Options;
             dest.Dimension4Options = src.Dimension4Options;
        }
    }
}