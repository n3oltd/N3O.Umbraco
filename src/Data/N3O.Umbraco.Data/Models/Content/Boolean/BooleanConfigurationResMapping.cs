using System.Collections.Generic;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Data.Models;

public class BooleanConfigurationResMapping : ContentPropertyConfigurationMapping<BooleanConfigurationRes> {
    public BooleanConfigurationResMapping(IContentTypeService contentTypeService,
                                          IEnumerable<IContentPropertyValidator> validators) 
        : base(contentTypeService, validators) { }

    public override void Map(ContentPropertyConfiguration src, BooleanConfigurationRes dest, MapperContext ctx) {
        MapConfiguration(src.ContentTypeAlias, src.PropertyAlias, dest);
    }
}