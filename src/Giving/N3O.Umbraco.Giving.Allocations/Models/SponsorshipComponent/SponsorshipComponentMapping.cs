using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Lookups;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Extensions;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class SponsorshipComponentMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<SponsorshipComponent, SponsorshipComponentRes>((_, _) => new SponsorshipComponentRes(), Map);
    }

    // Umbraco.Code.MapAll -Id -Name
    private void Map(SponsorshipComponent src, SponsorshipComponentRes dest, MapperContext ctx) {
        ctx.Map<INamedLookup, NamedLookupRes>(src, dest);
        dest.Mandatory = src.Mandatory;
        dest.Pricing = src.Pricing.IfNotNull(ctx.Map<IPricing, PricingRes>);
    }
}
