using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Lookups;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Models;

public class SponsorshipDurationMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<SponsorshipDuration, SponsorshipDurationRes>((_, _) => new SponsorshipDurationRes(), Map);
    }

    // Umbraco.Code.MapAll -Id -Name
    private void Map(SponsorshipDuration src, SponsorshipDurationRes dest, MapperContext ctx) {
        ctx.Map<INamedLookup, NamedLookupRes>(src, dest);
        
        dest.Months = src.Months;
    }
}
