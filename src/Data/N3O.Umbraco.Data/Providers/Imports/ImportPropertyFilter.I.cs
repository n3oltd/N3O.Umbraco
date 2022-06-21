using N3O.Umbraco.Data.Models;

namespace N3O.Umbraco.Data.Filters;

public interface IImportPropertyFilter {
    bool CanImport(UmbracoPropertyInfo propertyInfo);
    bool IsFilter(UmbracoPropertyInfo propertyInfo);
}
