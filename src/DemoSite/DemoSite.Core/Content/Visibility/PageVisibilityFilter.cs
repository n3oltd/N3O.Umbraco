using N3O.Umbraco.Content;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace DemoSite.Core.Content {
    public class PageVisibilityFilter : IContentVisibilityFilter {
        public bool IsFilterFor(IPublishedContent content) {
            return content is IPage;
        }

        public bool IsVisible(IPublishedContent content) {
            return !((IPage) content).HidePage;
        }
    }
}