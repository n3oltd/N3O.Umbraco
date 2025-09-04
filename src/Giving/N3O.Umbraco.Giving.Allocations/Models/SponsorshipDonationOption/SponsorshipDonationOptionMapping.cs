using N3O.Umbraco.Giving.Allocations.Content;
using N3O.Umbraco.Lookups;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class SponsorshipDonationOptionMapping : IMapDefinition {
    private readonly ILookups _lookups;
    
    public SponsorshipDonationOptionMapping(ILookups lookups) {
        _lookups = lookups;
    }

    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<SponsorshipDonationOptionContent, SponsorshipDonationOptionRes>((_, _) => new SponsorshipDonationOptionRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(SponsorshipDonationOptionContent src, SponsorshipDonationOptionRes dest, MapperContext ctx) {
        dest.Scheme = src.GetScheme(_lookups);
    }
}
