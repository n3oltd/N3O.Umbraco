using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Settings.Tracking.Alias)]
public class TrackingContent : UmbracoContent<TrackingContent> {
    public GoogleAnalyticsTrackingContent GoogleAnalytics => Content().GetSingleChildOfTypeAs<GoogleAnalyticsTrackingContent>();
    public MetaTrackingContent Meta => Content().GetSingleChildOfTypeAs<MetaTrackingContent>();
    public TikTokTrackingContent TikTok => Content().GetSingleChildOfTypeAs<TikTokTrackingContent>();
}