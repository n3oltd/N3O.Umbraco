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
        // FLAGGED (CS0618): IDataTypeService.GetDataType(int) is obsolete (removed in Umbraco 18). The
        // async replacement is GetAsync(Guid), but propertyType only exposes an int DataTypeId (no key here),
        // and this method is a public static iterator (yield return) that cannot be made async. Converting
        // would change the public IEnumerable<UmbracoPropertyInfo> signature and ripple to callers outside
        // this project. Left as-is to avoid breaking the build / behaviour.
        var dataType = dataTypeService.GetDataType(propertyType.DataTypeId);
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
