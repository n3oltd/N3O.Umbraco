using N3O.Umbraco.Constants;
using N3O.Umbraco.OpenGraph;
using N3O.Umbraco.Pages;
using N3O.Umbraco.StructuredData;

namespace N3O.Umbraco.Extensions;

public static class PageViewModelExtensions {
    public static OpenGraphData OpenGraph(this IPageViewModel pageViewModel) {
        return pageViewModel.ModulesData.Get<OpenGraphData>(PageModules.Keys.OpenGraph);
    }
    
    public static StructuredDataCode StructuredData(this IPageViewModel pageViewModel) {
        return pageViewModel.ModulesData.Get<StructuredDataCode>(PageModules.Keys.StructuredData);
    }
}
