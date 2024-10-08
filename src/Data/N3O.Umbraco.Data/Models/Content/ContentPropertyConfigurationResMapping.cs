using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Data.Models;

public abstract class ContentPropertyConfigurationMapping<T> : IMapDefinition where T : new() {
    private readonly IContentTypeService _contentTypeService;

    protected ContentPropertyConfigurationMapping(IContentTypeService contentTypeService,
                                                  IEnumerable<IContentPropertyValidator> validators) {
        _contentTypeService = contentTypeService;
        Validators = validators;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ContentPropertyConfiguration, T>((_, _) => new T(), Map);
    }

    protected void MapConfiguration(string contentTypeAlias, string propertyTypeAlias, ContentPropertyConfigurationRes dest) {
        var validator = Validators.SingleOrDefault(x => x.IsValidator(contentTypeAlias, propertyTypeAlias));
        
        if (validator != null) {
            var propertyType = GetPropertyType(contentTypeAlias, propertyTypeAlias);
            
            validator.PopulatePropertyConfiguration(propertyType, dest);
        }
    }
    
    private IPropertyType GetPropertyType(string contentTypeAlias, string propertyTypeAlias) {
        var contentType = _contentTypeService.Get(contentTypeAlias);
        
        return contentType.CompositionPropertyTypes.Single(x => x.Alias == propertyTypeAlias);
    }

    public abstract void Map(ContentPropertyConfiguration src, T dest, MapperContext ctx);
    
    public IEnumerable<IContentPropertyValidator> Validators { get; }
}