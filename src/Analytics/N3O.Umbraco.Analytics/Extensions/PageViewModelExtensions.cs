using N3O.Umbraco.Analytics.Models;
using N3O.Umbraco.Pages;

namespace N3O.Umbraco.Analytics.Extensions {
    public static class PageViewModelExtensions {
        public static Code DataLayer(this IPageViewModel pageViewModel) {
            return pageViewModel.ModulesData.Get<Code>(AnalyticsConstants.PageModuleKeys.DataLayer);
        }
    
        public static TagManagerCode TagManager(this IPageViewModel pageViewModel) {
            return pageViewModel.ModulesData.Get<TagManagerCode>(AnalyticsConstants.PageModuleKeys.TagManager);
        }
    }
}
