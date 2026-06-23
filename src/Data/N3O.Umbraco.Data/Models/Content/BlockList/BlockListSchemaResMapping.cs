using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;
using PropertyType = N3O.Umbraco.Data.Lookups.PropertyType;

namespace N3O.Umbraco.Data.Models;

public class BlockListSchemaResMapping : IMapDefinition {
    private readonly IContentTypeService _contentTypeService;
    private readonly IEnumerable<PropertyType> _propertyTypes;

    public BlockListSchemaResMapping(IContentTypeService contentTypeService, ILookups lookups) {
        _contentTypeService = contentTypeService;
        _propertyTypes = lookups.GetAll<PropertyType>();
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<PublishedContentProperty, BlockListSchemaRes>((_, _) => new BlockListSchemaRes(), Map);
    }

    private void Map(PublishedContentProperty src, BlockListSchemaRes dest, MapperContext ctx) {
        var blockListConfiguration = src.Property.PropertyType.DataType.ConfigurationAs<BlockListConfiguration>();

        var items = new List<BlockListSchemaItemRes>();

        foreach (var block in blockListConfiguration?.Blocks.OrEmpty()) {
            var contentType = _contentTypeService.Get(block.ContentElementTypeKey);

            if (contentType != null) {
                items.Add(PopulateContentTypes(ctx, contentType.Alias));
            }
        }
        
        dest.Items = items;
    }

    private BlockListSchemaItemRes PopulateContentTypes(MapperContext ctx, string contentTypeAlias) {
        var contentType = _contentTypeService.Get(contentTypeAlias);
        
        var properties = new List<BlockListSchemaPropertyRes>();

        foreach (var propertyType in contentType.CompositionPropertyTypes.OrEmpty()) {
            properties.Add(GetBlockListSchemaPropertyRes(ctx, contentTypeAlias, propertyType));
        }
        
        var res = new BlockListSchemaItemRes();
        res.ContentTypeAlias = contentType.Alias;
        res.Properties = properties;

        return res;
    }

    private BlockListSchemaPropertyRes GetBlockListSchemaPropertyRes(MapperContext ctx,
                                                               string contentTypeAlias,
                                                               IPropertyType propertyType)  {
        var type = _propertyTypes.SingleOrDefault(x => x.EditorAliases.Contains(propertyType.PropertyEditorAlias));
            
        var res = new BlockListSchemaPropertyRes();
        res.Type = type;
        res.Alias = propertyType.Alias;
        
        res.Configuration = type.GetConfigurationRes(ctx, contentTypeAlias, propertyType.Alias);

        return res;
    }
}