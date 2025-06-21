using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Settings.Payments.BannerAdverts.Alias)]
public class BannerAdvertContent : UmbracoContent<BannerAdvertContent> {
    public MediaWithCrops Image => GetValue(x => x.Image);
    public Link Link => GetValue(x => x.Link);
}