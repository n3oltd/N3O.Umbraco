using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Settings.Tracking.GoogleAnalytics)]
public class GoogleAnalyticsTrackingContent : UmbracoContent<GoogleAnalyticsTrackingContent> {
    public string MeasurementId => GetValue(x => x.MeasurementId);
}