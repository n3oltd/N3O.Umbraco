using N3O.Umbraco.Giving.Allocations.Lookups;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models;

public class SponsorshipDurationDataMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<SponsorshipDuration, SponsorshipDurationData>((_, _) => new SponsorshipDurationData(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(SponsorshipDuration src, SponsorshipDurationData dest, MapperContext ctx) {
        dest.Id = src.Id;
        dest.Name = src.Name;
        dest.Months = src.Months;
    }
}
