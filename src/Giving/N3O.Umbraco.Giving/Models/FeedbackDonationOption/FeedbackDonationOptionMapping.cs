using N3O.Umbraco.Extensions;
using N3O.Umbraco.Giving.Content;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Giving.Models;

public class FeedbackDonationOptionMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<FeedbackDonationOptionContent, FeedbackDonationOptionRes>((_, _) => new FeedbackDonationOptionRes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(FeedbackDonationOptionContent src, FeedbackDonationOptionRes dest, MapperContext ctx) {
        dest.Scheme = src.Scheme;
    }
}