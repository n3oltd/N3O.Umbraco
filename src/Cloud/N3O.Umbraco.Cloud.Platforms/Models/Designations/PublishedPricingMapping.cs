using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Giving.Allocations.Models;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedPricingMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IPrice, PublishedPrice>((_, _) => new PublishedPrice(), MapPrice);
        mapper.Define<IPricingRule, PublishedPricingRule>((_, _) => new PublishedPricingRule(), MapPricingRule);
    }

    private void MapPricingRule(IPricingRule src, PublishedPricingRule dest, MapperContext ctx) {
        dest.Price = ctx.Map<IPrice, PublishedPrice>(src);
        dest.FundDimensions = ctx.Map<IFundDimensionValues, PublishedFundDimensionValues>(src.FundDimensions);
    }

    private void MapPrice(IPrice src, PublishedPrice dest, MapperContext ctx) {
        dest.Amount = (double) src.Amount;
        dest.Locked = src.Locked;
    }
}
