using N3O.Umbraco.CrowdFunding;
using System.Collections.Generic;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Crowdfunding.Models;

public class TextareaConfigurationResMapping : ContentPropertyConfigurationMapping<TextareaConfigurationRes> {
    public TextareaConfigurationResMapping(IContentTypeService contentTypeService,
                                          IEnumerable<IContentPropertyValidator> validators) 
        : base(contentTypeService, validators) { }

    public override void Map(PublishedContentProperty src, TextareaConfigurationRes dest, MapperContext ctx) {
        MapConfiguration(src.ContentTypeAlias, src.Property.Alias, dest);
    }
}