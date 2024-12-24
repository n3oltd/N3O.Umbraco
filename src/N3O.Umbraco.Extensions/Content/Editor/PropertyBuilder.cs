using N3O.Umbraco.Extensions;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Content;

public abstract class PropertyBuilder : IPropertyBuilder {
    private readonly IContentTypeService _contentTypeService;

    protected PropertyBuilder(IContentTypeService contentTypeService) {
        _contentTypeService = contentTypeService;
    }
    
    public virtual (object, IPropertyType) Build(string propertyAlias, string contentTypeAlias) {
        var propertyType = GetPropertyType(propertyAlias, contentTypeAlias);
        
        return (Value, propertyType);
    }

    protected IPropertyType GetPropertyType(string propertyAlias, string contentTypeAlias) {
        var contentType = _contentTypeService.Get(contentTypeAlias);
        var propertyType = contentType.PropertyTypes.SingleOrDefault(x => x.Alias.EqualsInvariant(propertyAlias));

        return propertyType;
    }

    protected object Value { get; set; }
}
