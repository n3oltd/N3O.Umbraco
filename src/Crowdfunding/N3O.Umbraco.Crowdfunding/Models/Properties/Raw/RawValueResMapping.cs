using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.Crowdfunding.Models;

public class RawValueResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IPublishedProperty, RawValueRes>((_, _) => new RawValueRes(), Map);
    }

    private void Map(IPublishedProperty src, RawValueRes dest, MapperContext ctx) {
        dest.Value = src.GetValue() as HtmlEncodedString;
    }
}