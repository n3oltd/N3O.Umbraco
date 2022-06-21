using N3O.Umbraco.Data.Models;

namespace N3O.Umbraco.Data.Filters;

public interface IExportPropertyFilter {
    bool CanExport(UmbracoPropertyInfo propertyInfo);
    bool IsFilter(UmbracoPropertyInfo propertyInfo);
}
