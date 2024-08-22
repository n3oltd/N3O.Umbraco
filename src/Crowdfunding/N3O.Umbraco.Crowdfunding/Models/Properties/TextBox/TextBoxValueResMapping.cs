using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Crowdfunding.Models;

public class TextBoxValueResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<PublishedContentProperty, TextBoxValueRes>((_, _) => new TextBoxValueRes(), Map);
    }

    private void Map(PublishedContentProperty src, TextBoxValueRes dest, MapperContext ctx) {
        dest.Value = src.Property.GetValue() as string;
        dest.Configuration = ctx.Map<PublishedContentProperty, TextBoxConfigurationRes>(src);
    }
}