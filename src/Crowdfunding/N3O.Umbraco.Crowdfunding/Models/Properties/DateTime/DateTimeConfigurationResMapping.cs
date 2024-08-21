using N3O.Umbraco.CrowdFunding;
using System.Collections.Generic;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Crowdfunding.Models;

public class DateTimeConfigurationResMapping : ContentPropertyConfigurationMapping<DateTimeConfigurationRes> {
    public DateTimeConfigurationResMapping(IContentTypeService contentTypeService,
                                          IEnumerable<IContentPropertyValidator> validators) 
        : base(contentTypeService, validators) { }

    public override void Map(PublishedContentProperty src, DateTimeConfigurationRes dest, MapperContext ctx) {
        MapConfiguration(src.ContentTypeAlias, src.Property.Alias, dest);
    }
}