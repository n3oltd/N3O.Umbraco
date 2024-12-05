using N3O.Umbraco.Giving.Allocations.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class SponsorshipDonationOptionMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<SponsorshipDonationOptionContent, SponsorshipDonationOptionRes>((_, _) => new SponsorshipDonationOptionRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(SponsorshipDonationOptionContent src, SponsorshipDonationOptionRes dest, MapperContext ctx) {
        dest.Scheme = src.Scheme;
    }
}
