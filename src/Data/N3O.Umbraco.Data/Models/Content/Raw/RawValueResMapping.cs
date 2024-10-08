using N3O.Umbraco.Data.Lookups;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Strings;

namespace N3O.Umbraco.Data.Models;

public class RawValueResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<PublishedContentProperty, RawValueRes>((_, _) => new RawValueRes(), Map);
    }

    private void Map(PublishedContentProperty src, RawValueRes dest, MapperContext ctx) {
        dest.Value = src.Property.GetValue() as HtmlEncodedString;
        dest.Configuration = (RawConfigurationRes) PropertyTypes.Raw.GetConfigurationRes(ctx,
                                                                                         src.ContentTypeAlias,
                                                                                         src.Property.Alias);
    }
}