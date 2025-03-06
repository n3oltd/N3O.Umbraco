using N3O.Umbraco.Elements.Clients;
using N3O.Umbraco.Elements.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models.CheckoutProfile;

public class AdvertContentMapping : IMapDefinition {
    private readonly IUrlBuilder _urlBuilder;

    public AdvertContentMapping(IUrlBuilder urlBuilder) {
        _urlBuilder = urlBuilder;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<AdvertContentElement, AdvertsSettings>((_, _) => new AdvertsSettings(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(AdvertContentElement src, AdvertsSettings dest, MapperContext ctx) {
        dest.ImageUrl = _urlBuilder.Root().AppendPathSegment(src.Image.Src);
        dest.Link = src.Link.Content?.AbsoluteUrl() ?? src.Link.Url;
    }
}