using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding.Models;

public class TextBoxValueResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IPublishedProperty, TextBoxValueRes>((_, _) => new TextBoxValueRes(), Map);
    }

    private void Map(IPublishedProperty src, TextBoxValueRes dest, MapperContext ctx) {
        dest.Value = src.GetValue() as string;
    }
}