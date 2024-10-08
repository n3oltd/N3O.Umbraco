using System.Collections.Generic;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Data.Models;

public class NumericConfigurationResMapping : ContentPropertyConfigurationMapping<NumericConfigurationRes> {
    public NumericConfigurationResMapping(IContentTypeService contentTypeService,
                                          IEnumerable<IContentPropertyValidator> validators) 
        : base(contentTypeService, validators) { }

    public override void Map(ContentPropertyConfiguration src, NumericConfigurationRes dest, MapperContext ctx) {
        MapConfiguration(src.ContentTypeAlias, src.PropertyAlias, dest);
    }
}