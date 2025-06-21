using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedGoogleAnalyticsTrackingMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<GoogleAnalyticsTrackingContent, PublishedGoogleAnalyticsTracking>((_, _) => new PublishedGoogleAnalyticsTracking(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(GoogleAnalyticsTrackingContent src, PublishedGoogleAnalyticsTracking dest, MapperContext ctx) {
        dest.MeasurementId = src.MeasurementId;
    }
}