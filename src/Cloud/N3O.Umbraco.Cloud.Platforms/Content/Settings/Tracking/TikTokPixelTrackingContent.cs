using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.TikTokPixelTracking.Alias)]
public class TikTokPixelTrackingContent : UmbracoContent<TikTokPixelTrackingContent> {
    public string PixelId => GetValue(x => x.PixelId);
}