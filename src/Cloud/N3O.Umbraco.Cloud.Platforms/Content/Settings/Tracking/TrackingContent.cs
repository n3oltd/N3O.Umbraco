using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;

namespace N3O.Umbraco.Cloud.Platforms.Content.Settings;

[UmbracoContent(PlatformsConstants.Tracking.Alias)]
public class TrackingContent : UmbracoContent<TrackingContent> {
    public GoogleAnalyticsTrackingContent Google => Content().GetSingleChildOfTypeAs<GoogleAnalyticsTrackingContent>();
    public MetaPixelTrackingContent Meta => Content().GetSingleChildOfTypeAs<MetaPixelTrackingContent>();
    public TikTokPixelTrackingContent TikTok => Content().GetSingleChildOfTypeAs<TikTokPixelTrackingContent>();
}