using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.MetaPixelTracking.Alias)]
public class MetaPixelTrackingContent : UmbracoContent<MetaPixelTrackingContent> {
    public string PixelId => GetValue(x => x.PixelId);
}