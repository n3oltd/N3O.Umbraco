using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;
using PropertyType = N3O.Umbraco.Crowdfunding.Lookups.PropertyType;

namespace N3O.Umbraco.Crowdfunding.Models;

public class NestedSchemaResMapping : IMapDefinition {
    private readonly IContentTypeService _contentTypeService;
    private readonly IEnumerable<PropertyType> _propertyTypes;

    public NestedSchemaResMapping(IContentTypeService contentTypeService, ILookups lookups) {
        _contentTypeService = contentTypeService;
        _propertyTypes = lookups.GetAll<PropertyType>();
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<IPublishedProperty, NestedSchemaRes>((_, _) => new NestedSchemaRes(), Map);
    }

    private void Map(IPublishedProperty src, NestedSchemaRes dest, MapperContext ctx) {
        var nestedConfiguration = src.PropertyType.DataType.ConfigurationAs<NestedContentConfiguration>();
        
        var items = new List<NestedSchemaItemRes>();

        foreach (var nestedContentType in nestedConfiguration?.ContentTypes.OrEmpty()) {
            items.Add(PopulateContentTypes(nestedContentType.Alias));
        }
        
        dest.Items = items;
    }

    private NestedSchemaItemRes PopulateContentTypes(string contentTypeAlias) {
        var contentType = _contentTypeService.Get(contentTypeAlias);
        
        var properties = new List<NestedSchemaPropertyRes>();

        foreach (var propertyType in contentType.CompositionPropertyTypes.OrEmpty()) {
            properties.Add(GetNestedSchemaPropertyRes(propertyType));
        }
        
        var res = new NestedSchemaItemRes();
        res.ContentTypeAlias = contentType.Alias;
        res.Properties = properties;

        return res;
    }

    private NestedSchemaPropertyRes GetNestedSchemaPropertyRes(IPropertyType propertyType)  {
        var type = _propertyTypes.SingleOrDefault(x => x.EditorAliases.Contains(propertyType.PropertyEditorAlias));
            
        var res = new NestedSchemaPropertyRes();
        res.Type = type;
        res.Alias = propertyType.Alias;

        return res;
    }
}