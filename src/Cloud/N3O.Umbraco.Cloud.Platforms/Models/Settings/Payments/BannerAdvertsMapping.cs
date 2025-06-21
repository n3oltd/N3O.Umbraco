using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Extensions;
using System.Linq;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class BannerAdvertsMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<BannerAdvertsContent, PublishedBannerAdverts>((_, _) => new PublishedBannerAdverts(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(BannerAdvertsContent src, PublishedBannerAdverts dest, MapperContext ctx) {
        dest.Adverts = src.Adverts.OrEmpty().Select(ctx.Map<BannerAdvertContent, PublishedBannerAdvert>).ToList();
    }
}