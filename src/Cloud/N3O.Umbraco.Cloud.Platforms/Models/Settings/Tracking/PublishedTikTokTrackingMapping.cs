using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedTikTokTrackingMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<TikTokTrackingContent, PublishedTikTokTracking>((_, _) => new PublishedTikTokTracking(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(TikTokTrackingContent src, PublishedTikTokTracking dest, MapperContext ctx) {
        dest.PixelId = src.PixelId;
    }
}