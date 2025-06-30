using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedSuggestedAmountMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<SuggestedAmountContent, PublishedSuggestedAmount>((_, _) => new PublishedSuggestedAmount(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(SuggestedAmountContent src, PublishedSuggestedAmount dest, MapperContext ctx) {
        dest.Amount = (double) src.Amount;
        dest.Description = src.Description;
    }
}
