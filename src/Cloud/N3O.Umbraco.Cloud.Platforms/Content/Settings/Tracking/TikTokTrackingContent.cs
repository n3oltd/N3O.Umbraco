using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Settings.Tracking.TikTok)]
public class TikTokTrackingContent : UmbracoContent<TikTokTrackingContent> {
    public string PixelId => GetValue(x => x.PixelId);
}