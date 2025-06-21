using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedTrackingMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<TrackingContent, PublishedTracking>((_, _) => new PublishedTracking(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(TrackingContent src, PublishedTracking dest, MapperContext ctx) {
        dest.GoogleAnalytics = ctx.Map<GoogleAnalyticsTrackingContent, PublishedGoogleAnalyticsTracking>(src.GoogleAnalytics);
        dest.Meta = ctx.Map<MetaTrackingContent, PublishedMetaTracking>(src.Meta);
        dest.TikTok = ctx.Map<TikTokTrackingContent, PublishedTikTokTracking>(src.TikTok);
    }
}