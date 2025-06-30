using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Settings.Tracking.Meta)]
public class MetaTrackingContent : UmbracoContent<MetaTrackingContent> {
    public string PixelId => GetValue(x => x.PixelId);
}