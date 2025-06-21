using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Giving.Allocations.Models;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedPricingRuleMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IPricingRule, PublishedPricingRule>((_, _) => new PublishedPricingRule(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(IPricingRule src, PublishedPricingRule dest, MapperContext ctx) {
        dest.Price = ctx.Map<IPrice, PublishedPrice>(src);
        dest.FundDimensions = ctx.Map<IFundDimensionValues, PublishedFundDimensionValues>(src.FundDimensions);
    }
}
