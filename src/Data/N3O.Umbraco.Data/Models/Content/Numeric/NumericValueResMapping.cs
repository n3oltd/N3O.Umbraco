using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Data.Models;

public class NumericValueResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<PublishedContentProperty, NumericValueRes>((_, _) => new NumericValueRes(), Map);
    }

    private void Map(PublishedContentProperty src, NumericValueRes dest, MapperContext ctx) {
        dest.Value = (decimal?) src.Property.GetValue();
        dest.Configuration = ctx.Map<PublishedContentProperty, NumericConfigurationRes>(src);
    }
}