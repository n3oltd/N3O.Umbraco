using Microsoft.AspNetCore.Hosting;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class BannerAdvertMapping : IMapDefinition {
    private readonly IContentLocator _contentLocator;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public BannerAdvertMapping(IContentLocator contentLocator, IWebHostEnvironment webHostEnvironment) {
        _contentLocator = contentLocator;
        _webHostEnvironment = webHostEnvironment;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<BannerAdvertContent, PublishedBannerAdvert>((_, _) => new PublishedBannerAdvert(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(BannerAdvertContent src, PublishedBannerAdvert dest, MapperContext ctx) {
        dest.Image = src.Image.GetPublishedUri(_contentLocator, _webHostEnvironment);
        dest.Link = src.Link.GetPublishedUri();
    }
}