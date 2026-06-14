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
        // IPropertyType.DataTypeKey (Guid) is available in v17. GetAsync(Guid) is the non-deprecated
        // replacement. GetAwaiter().GetResult() is safe here: ASP.NET Core thread pool has no
        // SynchronizationContext, and data type lookups are backed by HybridCache (fast in-memory).
        var dataType = dataTypeService.GetAsync(propertyType.DataTypeKey).GetAwaiter().GetResult();
        var elements = new List<ElementInfo>();
        
        if (propertyType.IsBlockList()) {
            var blockListConfiguration = dataType.ConfigurationAs<BlockListConfiguration>();

            foreach (var block in blockListConfiguration?.Blocks.OrEmpty()) {
                var blockContentType = contentTypeService.Get(block.ContentElementTypeKey);

                if (blockContentType != null) {
                    elements.Add(new ElementInfo(blockContentType,
                                                 GetUmbracoProperties(blockContentType,
                                                                      dataTypeService,
                                                                      contentTypeService)));
                }
            }
        }

        return new UmbracoPropertyInfo(contentType, propertyType, group, dataType, elements);
    }
}
