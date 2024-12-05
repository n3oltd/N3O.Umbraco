using N3O.Umbraco.Giving.Allocations.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Allocations.Models;

public class FeedbackDonationOptionMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FeedbackDonationOptionContent, FeedbackDonationOptionRes>((_, _) => new FeedbackDonationOptionRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(FeedbackDonationOptionContent src, FeedbackDonationOptionRes dest, MapperContext ctx) {
        dest.Scheme = src.Scheme;
    }
}