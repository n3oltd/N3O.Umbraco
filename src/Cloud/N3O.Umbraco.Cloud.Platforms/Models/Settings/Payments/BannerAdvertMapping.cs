using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using System;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class BannerAdvertMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<BannerAdvertContent, PublishedBannerAdvert>((_, _) => new PublishedBannerAdvert(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(BannerAdvertContent src, PublishedBannerAdvert dest, MapperContext ctx) {
        var imageUrl = src.Image.GetCropUrl(urlMode: UrlMode.Absolute);
        var link = src.Link.Content?.AbsoluteUrl() ?? src.Link.Url;
        
        dest.Image = new Uri(imageUrl);
        dest.Link = new Uri(link);
    }
}