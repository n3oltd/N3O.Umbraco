using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.BannerAdverts.Alias)]
public class BannerAdvertsContent : UmbracoContent<BannerAdvertsContent> {
    public IEnumerable<BannerAdvertContent> Adverts => Content().Children().OrEmpty().As<BannerAdvertContent>();
}