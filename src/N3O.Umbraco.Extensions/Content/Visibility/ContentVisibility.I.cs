using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Content;

public interface IContentVisibility {
    bool IsVisible(IPublishedContent content);
}
