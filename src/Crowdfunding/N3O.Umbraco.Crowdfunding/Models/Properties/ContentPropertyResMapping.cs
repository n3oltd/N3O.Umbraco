using N3O.Umbraco.CrowdFunding;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Services;
using PropertyType = N3O.Umbraco.Crowdfunding.Lookups.PropertyType;

namespace N3O.Umbraco.Crowdfunding.Models;

public class ContentPropertyResMapping : IMapDefinition {
    private readonly IContentTypeService _contentTypeService;
    private readonly ILookups _lookups;
    private readonly IEnumerable<IContentPropertyValidator> _contentPropertyValidators;

    public ContentPropertyResMapping(IContentTypeService contentTypeService,
                                     ILookups lookups,
                                     IEnumerable<IContentPropertyValidator> contentPropertyValidators) {
        _contentTypeService = contentTypeService;
        _lookups = lookups;
        _contentPropertyValidators = contentPropertyValidators;
    }

    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<PublishedContentProperty, ContentPropertyRes>((_, _) => new ContentPropertyRes(), Map);
    }

    private void Map(PublishedContentProperty src, ContentPropertyRes dest, MapperContext ctx) {
        var propertyTypes = _lookups.GetAll<PropertyType>();
        var propertyType = propertyTypes.SingleOrDefault(x => x.EditorAliases.Contains(src.Property.PropertyType.EditorAlias));

        if (propertyType == null) {
            throw new Exception($"Could not resolve property type for property editor {src.Property.PropertyType.EditorAlias}");
        }

        dest.Alias = src.Property.Alias;
        dest.Type = propertyType;
        dest.PropertyValue = PopulatePropertyValueRes(ctx, propertyType, src.Property);
        dest.PropertyCriteria = PopulatePropertyCriteriaRes(src);
    }

    private ContentPropertyValueRes PopulatePropertyValueRes(MapperContext ctx, PropertyType propertyType, IPublishedProperty src) {
        var dest = new ContentPropertyValueRes();

        propertyType.PopulateValueRes(ctx, src, dest);

        return dest;
    }
    
    private ContentPropertyCriteriaRes PopulatePropertyCriteriaRes(PublishedContentProperty publishedContentProperty) {
        var validator = _contentPropertyValidators.SingleOrDefault(x => x.IsValidator(publishedContentProperty.ContentTypeAlias,
                                                                                      publishedContentProperty.Property.Alias));

        var res = new ContentPropertyCriteriaRes();

        if (validator.HasValue()) {
            var propertyType = GetPropertyType(publishedContentProperty.ContentTypeAlias, publishedContentProperty.Property.Alias);
            
            validator.PopulateContentPropertyCriteriaRes(propertyType, res);
        }
        
        return res;
    }

    private IPropertyType GetPropertyType(string contentTypeAlias, string propertyTypeAlias) {
        var contentType = _contentTypeService.Get(contentTypeAlias);
        
        return contentType.CompositionPropertyTypes.Single(x => x.Alias == propertyTypeAlias);
    }
}