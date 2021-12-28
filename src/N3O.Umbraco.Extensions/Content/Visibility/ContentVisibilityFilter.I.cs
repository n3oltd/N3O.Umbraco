using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Content {
    public interface IContentVisibilityFilter {
        bool IsFilterFor(IPublishedContent content);
        bool IsVisible(IPublishedContent content);
    }
}