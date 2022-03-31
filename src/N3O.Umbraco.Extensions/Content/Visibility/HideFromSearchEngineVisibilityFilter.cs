using N3O.Umbraco.Extensions;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Content {
    public class HideFromSearchEngineVisibilityFilter : IContentVisibilityFilter {
        public bool IsFilterFor(IPublishedContent content) => true;

        public bool IsVisible(IPublishedContent content) {
            return !content.GetType().ImplementsInterface<IHideFromSearchEngines>();
        }
    }
}