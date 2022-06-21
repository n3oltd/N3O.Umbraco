using Umbraco.Cms.Core.Models;

namespace N3O.Umbraco.Data.Filters;

public interface IExportContentFilter {
    bool AllowExports(IContent content);
    bool IsFilter(IContent content);
}
