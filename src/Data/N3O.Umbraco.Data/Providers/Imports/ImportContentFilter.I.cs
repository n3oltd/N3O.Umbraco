using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Data.Filters;

public interface IImportContentFilter {
    bool AllowImports(IContent content);
    bool IsFilter(IContent content);
}
