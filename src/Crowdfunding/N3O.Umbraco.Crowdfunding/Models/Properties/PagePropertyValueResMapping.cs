using N3O.Umbraco.Crowdfunding.Lookups;
using N3O.Umbraco.Lookups;
using System;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Crowdfunding.Models;

public class PagePropertyValueResMapping : IMapDefinition {
    private readonly ILookups _lookups;

    public PagePropertyValueResMapping(ILookups lookups) {
        _lookups = lookups;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IPublishedProperty, PagePropertyValueRes>((_, _) => new PagePropertyValueRes(), Map);
    }

    private void Map(IPublishedProperty src, PagePropertyValueRes dest, MapperContext ctx) {
        var propertyTypes = _lookups.GetAll<PropertyType>();
        var propertyType = propertyTypes.SingleOrDefault(x => x.EditorAliases.Contains(src.PropertyType.EditorAlias));

        if (propertyType == null) {
            throw new Exception($"Could not resolve property type for property editor {src.PropertyType.EditorAlias}");
        }

        dest.Alias = src.Alias;
        dest.Type = propertyType;

        propertyType.PopulateRes(ctx, src, dest);
    }
}