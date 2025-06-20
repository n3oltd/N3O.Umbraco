using MuslimHands.Website.Connect.Clients;
using N3O.Umbraco.Utilities;
using System;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class BannerAdvertMapping : IMapDefinition {
    private readonly IUrlBuilder _urlBuilder;

    public BannerAdvertMapping(IUrlBuilder urlBuilder) {
        _urlBuilder = urlBuilder;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<PlatformsBannerAdvert, PublishedBannerAdvert>((_, _) => new PublishedBannerAdvert(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(PlatformsBannerAdvert src, PublishedBannerAdvert dest, MapperContext ctx) {
        var imageUrl = _urlBuilder.Root().AppendPathSegment(src.Image.SrcUrl());
        var link = src.Link.Content?.AbsoluteUrl() ?? src.Link.Url;
        
        dest.Image = new Uri(imageUrl);
        dest.Link = new Uri(link);
    }
}