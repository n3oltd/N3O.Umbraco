using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;
using Umbraco.Extensions;

namespace N3O.Umbraco.Data.Extensions;

public static class ContentTypeExtensions {
    public static IEnumerable<UmbracoPropertyInfo> GetUmbracoProperties(this IContentType contentType,
                                                                        IDataTypeService dataTypeService,
                                                                        IContentTypeService contentTypeService) {
        foreach (var propertyGroup in contentType.CompositionPropertyGroups.OrEmpty().OrderBy(x => x.SortOrder)) {
            foreach (var propertyType in propertyGroup.PropertyTypes.OrEmpty().OrderBy(x => x.SortOrder)) {
                yield return GetPropertyInfo(dataTypeService,
                                             contentTypeService,
                                             contentType,
                                             propertyType,
                                             propertyGroup);
            }
        }

        foreach (var propertyType in contentType.NoGroupPropertyTypes.OrEmpty().OrderBy(x => x.SortOrder)) {
            yield return GetPropertyInfo(dataTypeService, contentTypeService, contentType, propertyType);
        }
    }

    private static UmbracoPropertyInfo GetPropertyInfo(IDataTypeService dataTypeService,
                                                       IContentTypeService contentTypeService,
                                                       IContentType contentType,
                                                       IPropertyType propertyType,
                                                       PropertyGroup group = null) {
        var dataType = dataTypeService.GetDataType(propertyType.DataTypeId);
        var nestedContent = new List<NestedContentInfo>();
        
        if (propertyType.IsNestedContent()) {
            var nestedContentConfiguration = dataType.ConfigurationAs<NestedContentConfiguration>();
            var nestedContentTypes = nestedContentConfiguration.ContentTypes
                                                               .Select(x => contentTypeService.Get(x.Alias))
                                                               .ToList();

            foreach (var nestedContentType in nestedContentTypes) {
                nestedContent.Add(new NestedContentInfo(nestedContentType,
                                                        GetUmbracoProperties(nestedContentType,
                                                                             dataTypeService,
                                                                             contentTypeService)));
            }
        }

        return new UmbracoPropertyInfo(contentType, propertyType, group, dataType, nestedContent);
    }
}
