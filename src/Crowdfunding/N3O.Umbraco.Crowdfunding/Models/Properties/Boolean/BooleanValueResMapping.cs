using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding.Models;

public class BooleanValueResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IPublishedProperty, BooleanValueRes>((_, _) => new BooleanValueRes(), Map);
    }

    private void Map(IPublishedProperty src, BooleanValueRes dest, MapperContext ctx) {
        dest.Value = (bool?) src.GetValue();
    }
}