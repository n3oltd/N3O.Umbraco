using System;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding.Models;

public class DateTimeValueResMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IPublishedProperty, DateTimeValueRes>((_, _) => new DateTimeValueRes(), Map);
    }

    private void Map(IPublishedProperty src, DateTimeValueRes dest, MapperContext ctx) {
        dest.Value = (DateTime?) src.GetValue() ;
    }
}