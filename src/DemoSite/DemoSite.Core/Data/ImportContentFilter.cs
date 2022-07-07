using N3O.Umbraco.Data.Filters;
using Umbraco.Cms.Core.Models;

namespace DemoSite.Data; 

public class ImportContentFilter : IImportContentFilter {
    public bool AllowImports(IContent content) => true;
    public bool IsFilter(IContent content) => true;
}