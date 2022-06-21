using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Content;

public class ContentVisibility : IContentVisibility {
    private readonly IReadOnlyList<IContentVisibilityFilter> _filters;

    public ContentVisibility(IEnumerable<IContentVisibilityFilter> filters) {
        _filters = filters.ToList();
    }

    public bool IsVisible(IPublishedContent content) {
        var isVisible = _filters.Where(x => x.IsFilterFor(content))
                                .All(x => x.IsVisible(content));

        return isVisible;
    }
}
