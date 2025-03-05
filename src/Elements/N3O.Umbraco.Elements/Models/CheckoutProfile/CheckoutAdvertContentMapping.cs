using N3O.Umbraco.Elements.Clients;
using N3O.Umbraco.Elements.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Utilities;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Elements.Models.CheckoutProfile;

public class CheckoutAdvertContentMapping : IMapDefinition {
    private readonly IUrlBuilder _urlBuilder;

    public CheckoutAdvertContentMapping(IUrlBuilder urlBuilder) {
        _urlBuilder = urlBuilder;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<CheckoutAdvertContentElement, CheckoutAdvertsSettings>((_, _) => new CheckoutAdvertsSettings(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(CheckoutAdvertContentElement src, CheckoutAdvertsSettings dest, MapperContext ctx) {
        dest.ImageUrl = _urlBuilder.Root().AppendPathSegment(src.Image.Src);
        dest.Link = src.Link.Content?.AbsoluteUrl() ?? src.Link.Url;
    }
}