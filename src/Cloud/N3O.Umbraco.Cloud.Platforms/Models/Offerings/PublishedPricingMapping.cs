using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Allocations.Models;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedPricingMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IPricing, PublishedPricing>((_, _) => new PublishedPricing(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(IPricing src, PublishedPricing dest, MapperContext ctx) {
        dest.Price = src.Price.IfNotNull(ctx.Map<IPrice, PublishedPrice>);
        dest.Rules = src.Rules.OrEmpty().Select(ctx.Map<IPricingRule, PublishedPricingRule>).ToList();
    }
}
