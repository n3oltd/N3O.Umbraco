using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Allocations.Models;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models;

public class SponsorshipComponentDataMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<SponsorshipComponent, SponsorshipComponentData>((_, _) => new SponsorshipComponentData(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(SponsorshipComponent src, SponsorshipComponentData dest, MapperContext ctx) {
        dest.Id = src.Id;
        dest.Name = src.Name;
        dest.Mandatory = src.Mandatory;
        dest.Pricing = ctx.Map<IPricing, PricingData>(src);
    }
}
