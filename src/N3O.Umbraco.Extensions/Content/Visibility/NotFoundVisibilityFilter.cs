using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Content;

public class NotFoundVisibilityFilter : IContentVisibilityFilter {
    private readonly IContentCache _contentCache;

    public NotFoundVisibilityFilter(IContentCache contentCache) {
        _contentCache = contentCache;
    }

    public bool IsFilterFor(IPublishedContent content) {
        var notFoundPage = _contentCache.Special(SpecialPages.NotFound);

        return content.Id == notFoundPage?.Id;
    }

    public bool IsVisible(IPublishedContent content) {
        return false;
    }
}
