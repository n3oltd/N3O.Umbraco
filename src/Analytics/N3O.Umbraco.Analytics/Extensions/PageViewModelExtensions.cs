using N3O.Umbraco.Analytics.Models;
using N3O.Umbraco.Pages;

namespace N3O.Umbraco.Analytics.Extensions {
    public static class PageViewModelExtensions {
        public static Code DataLayer(this IPageViewModel pageViewModel) {
            return pageViewModel.ModulesData.Get<Code>(AnalyticsConstants.PageModuleKeys.DataLayer);
        }
        
        public static Code GoogleAnalytics4(this IPageViewModel pageViewModel) {
            return pageViewModel.ModulesData.Get<Code>(AnalyticsConstants.PageModuleKeys.GoogleAnalytics4);
        }
    
        public static GoogleTagManagerCode GoogleTagManager(this IPageViewModel pageViewModel) {
            return pageViewModel.ModulesData.Get<GoogleTagManagerCode>(AnalyticsConstants.PageModuleKeys.GoogleTagManager);
        }
    }
}
