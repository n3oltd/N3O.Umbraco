using System.Collections.Generic;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Data.Models;

public class RawConfigurationResMapping : ContentPropertyConfigurationMapping<RawConfigurationRes> {
    public RawConfigurationResMapping(IContentTypeService contentTypeService,
                                      IEnumerable<IContentPropertyValidator> validators) 
        : base(contentTypeService, validators) { }

    public override void Map(PublishedContentProperty src, RawConfigurationRes dest, MapperContext ctx) {
        MapConfiguration(src.ContentTypeAlias, src.Property.Alias, dest);
    }
}