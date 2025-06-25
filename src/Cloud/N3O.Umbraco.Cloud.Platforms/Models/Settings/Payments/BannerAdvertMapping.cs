using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Media;
using System;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class BannerAdvertMapping : IMapDefinition {
    private readonly IMediaUrl _mediaUrl;

    public BannerAdvertMapping(IMediaUrl mediaUrl) {
        _mediaUrl = mediaUrl;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<BannerAdvertContent, PublishedBannerAdvert>((_, _) => new PublishedBannerAdvert(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(BannerAdvertContent src, PublishedBannerAdvert dest, MapperContext ctx) {
        dest.Image = _mediaUrl.GetMediaUrl(src.Image, urlMode: UrlMode.Absolute).IfNotNull(x => new Uri(x));
        dest.Link = src.Link.GetPublishedUri();
    }
}