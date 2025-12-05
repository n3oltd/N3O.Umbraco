using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Giving.Allocations.Models;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedPriceMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IPrice, PublishedPrice>((_, _) => new PublishedPrice(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(IPrice src, PublishedPrice dest, MapperContext ctx) {
        dest.Amount = (double) src.Amount;
        dest.Locked = src.Locked;
    }
}