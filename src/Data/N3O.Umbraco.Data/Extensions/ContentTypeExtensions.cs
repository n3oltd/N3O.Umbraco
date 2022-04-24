using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Data.Extensions {
    public static class ContentTypeExtensions {
        public static IEnumerable<UmbracoPropertyInfo> GetUmbracoProperties(this IContentType contentType,
                                                                            IDataTypeService dataTypeService) {
            foreach (var propertyGroup in contentType.CompositionPropertyGroups.OrEmpty().OrderBy(x => x.SortOrder)) {
                foreach (var propertyType in propertyGroup.PropertyTypes.OrEmpty().OrderBy(x => x.SortOrder)) {
                    yield return GetPropertyInfo(dataTypeService, contentType, propertyType, propertyGroup);
                }
            }

            foreach (var propertyType in contentType.NoGroupPropertyTypes.OrEmpty().OrderBy(x => x.SortOrder)) {
                yield return GetPropertyInfo(dataTypeService, contentType, propertyType);
            }
        }

        private static UmbracoPropertyInfo GetPropertyInfo(IDataTypeService dataTypeService,
                                                           IContentType contentType,
                                                           IPropertyType propertyType,
                                                           PropertyGroup group = null) {
            var dataType = dataTypeService.GetDataType(propertyType.DataTypeId);

            return new UmbracoPropertyInfo(contentType, propertyType, group, dataType);
        }
    }
}