using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content.Settings;
using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedTrackingMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<TrackingContent, PublishedTracking>((_, _) => new PublishedTracking(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(TrackingContent src, PublishedTracking dest, MapperContext ctx) {
        if (src.Meta.HasValue(x => x.PixelId)) {
            dest.Meta = new PublishedMetaTracking();
            dest.Meta.PixelId = src.Meta.PixelId;
        }
        
        if (src.Google.HasValue(x => x.MeasurementId)) {
            dest.GoogleAnalytics = new PublishedGoogleAnalyticsTracking();
            dest.GoogleAnalytics.MeasurementId = src.Google.MeasurementId;
        }
        
        if (src.TikTok.HasValue(x => x.PixelId)) {
            dest.TikTok = new PublishedTikTokTracking();
            dest.TikTok.PixelId = src.TikTok.PixelId;
        }
    }
}