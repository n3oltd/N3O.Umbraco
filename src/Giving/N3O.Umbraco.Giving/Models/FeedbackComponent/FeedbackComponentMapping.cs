using N3O.Umbraco.Giving.Lookups;
using N3O.Umbraco.Lookups;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackComponentMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FeedbackComponent, FeedbackComponentRes>((_, _) => new FeedbackComponentRes(), Map);
    }

    // Umbraco.Code.MapAll -Id -Name
    private void Map(FeedbackComponent src, FeedbackComponentRes dest, MapperContext ctx) {
        ctx.Map<INamedLookup, NamedLookupRes>(src, dest);
        dest.Mandatory = src.Mandatory;
        dest.Pricing = ctx.Map<IPricing, PricingRes>(src);
    }
}
