using N3O.Umbraco.Data.Lookups;
using System;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Data.Models;

public class DateTimeValueResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<PublishedContentProperty, DateTimeValueRes>((_, _) => new DateTimeValueRes(), Map);
    }

    private void Map(PublishedContentProperty src, DateTimeValueRes dest, MapperContext ctx) {
        dest.Value = (DateTime?) src.Property.GetValue();
        dest.Configuration = (DateTimeConfigurationRes) PropertyTypes.DateTime.GetConfigurationRes(ctx,
                                                                                                  src.ContentTypeAlias,
                                                                                                  src.Property.Alias);
    }
}