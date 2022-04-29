using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Data.Filters;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Data.Extensions {
    public static class UmbracoPropertyInfoExtensions {
        public static bool CanInclude(this UmbracoPropertyInfo propertyInfo,
                                      IEnumerable<IExportPropertyFilter> propertyFilters) {
            var filters = propertyFilters.OrEmpty().Where(x => x.IsFilter(propertyInfo)).ToList();
            
            foreach (var filter in filters) {
                if (!filter.CanExport(propertyInfo)) {
                    return false;
                }
            }

            return true;
        }
        
        public static bool CanInclude(this UmbracoPropertyInfo propertyInfo,
                                      IEnumerable<IImportPropertyFilter> propertyFilters) {
            var filters = propertyFilters.OrEmpty().Where(x => x.IsFilter(propertyInfo)).ToList();
            
            foreach (var filter in filters) {
                if (!filter.CanImport(propertyInfo)) {
                    return false;
                }
            }

            return true;
        }
        
        public static IReadOnlyList<Column> GetColumns(this UmbracoPropertyInfo propertyInfo,
                                                       IEnumerable<IPropertyConverter> converters) {
            var converter = converters.SingleOrDefault(x => x.IsConverter(propertyInfo));

            if (converter == null) {
                throw new Exception($"No property converter found for {propertyInfo.DataType.EditorAlias.Quote()}");
            }

            return converter.GetColumns(propertyInfo);
        }
        
        public static string GetName(this UmbracoPropertyInfo propertyInfo) {
            var name = propertyInfo.Group.HasValue() ? $"{propertyInfo.Group.Name}: " : null;
            name += propertyInfo.Type.Name;

            return name;
        }
    }
}