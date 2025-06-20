using MuslimHands.Website.Connect.Clients;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedTrackingMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<PlatformsTracking, PublishedTracking>((_, _) => new PublishedTracking(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(PlatformsTracking src, PublishedTracking dest, MapperContext ctx) {
        var metaPixelTracking = src.Child<PlatformsMetaPixelTracking>();
        var tikTokPixelTracking = src.Child<PlatformsTikTokPixelTracking>();
        var googleAnalyticsTracking = src.Child<PlatformsGoogleAnalyticsTracking>();
        
        if (metaPixelTracking.HasValue(x => x.PixelId)) {
            dest.Meta = new PublishedMetaTracking();
            dest.Meta.PixelId = metaPixelTracking.PixelId;
        }
        
        if (googleAnalyticsTracking.HasValue(x => x.MeasurementId)) {
            dest.GoogleAnalytics = new PublishedGoogleAnalyticsTracking();
            dest.GoogleAnalytics.MeasurementId = googleAnalyticsTracking.MeasurementId;
        }
        
        if (tikTokPixelTracking.HasValue(x => x.PixelId)) {
            dest.TikTok = new PublishedTikTokTracking();
            dest.TikTok.PixelId = tikTokPixelTracking.PixelId;
        }
    }
}