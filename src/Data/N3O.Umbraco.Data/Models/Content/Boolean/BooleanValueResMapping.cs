using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Data.Models;

public class BooleanValueResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<PublishedContentProperty, BooleanValueRes>((_, _) => new BooleanValueRes(), Map);
    }

    private void Map(PublishedContentProperty src, BooleanValueRes dest, MapperContext ctx) {
        dest.Value = (bool?) src.Property.GetValue();
        dest.Configuration = ctx.Map<PublishedContentProperty, BooleanConfigurationRes>(src);
    }
}