using N3O.Umbraco.Analytics.Models;
using N3O.Umbraco.Constants;
using N3O.Umbraco.Pages;

namespace N3O.Umbraco.Extensions {
    public static class PageViewModelExtensions {
        public static StructuredDataCode StructuredData(this IPageViewModel pageViewModel) {
            return pageViewModel.ModulesData.Get<StructuredDataCode>(PageModules.Keys.StructuredData);
        }
    }
}
