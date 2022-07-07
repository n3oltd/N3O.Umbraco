using N3O.Umbraco.Data.Filters;
using Umbraco.Cms.Core.Models;

namespace DemoSite.Data; 

public class ExportContentFilter : IExportContentFilter {
    public bool AllowExports(IContent content) => true;
    public bool IsFilter(IContent content) => true;
}