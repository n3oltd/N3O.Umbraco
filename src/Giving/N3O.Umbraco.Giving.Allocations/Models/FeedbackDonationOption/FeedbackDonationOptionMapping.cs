using N3O.Umbraco.Giving.Allocations.Content;
using N3O.Umbraco.Lookups;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class FeedbackDonationOptionMapping : IMapDefinition {
    private readonly ILookups _lookups;
    
    public FeedbackDonationOptionMapping(ILookups lookups) {
        _lookups = lookups;
    }

    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FeedbackDonationOptionContent, FeedbackDonationOptionRes>((_, _) => new FeedbackDonationOptionRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(FeedbackDonationOptionContent src, FeedbackDonationOptionRes dest, MapperContext ctx) {
        dest.Scheme = src.GetScheme(_lookups);
    }
}