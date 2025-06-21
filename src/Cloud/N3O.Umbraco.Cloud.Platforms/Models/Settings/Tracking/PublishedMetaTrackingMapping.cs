using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedMetaTrackingMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<MetaTrackingContent, PublishedMetaTracking>((_, _) => new PublishedMetaTracking(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(MetaTrackingContent src, PublishedMetaTracking dest, MapperContext ctx) {
        dest.PixelId = src.PixelId;
    }
}