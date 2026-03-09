using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms;

public abstract class PlatformsPageContentVisibilityFilter : IContentVisibilityFilter {
    private readonly IContentCache _contentCache;
    private readonly Lazy<IPlatformsPageAccessor> _platformsPageAccessor;

    protected PlatformsPageContentVisibilityFilter(IContentCache contentCache,
                                                   Lazy<IPlatformsPageAccessor> platformsPageAccessor) {
        _contentCache = contentCache;
        _platformsPageAccessor = platformsPageAccessor;
    }
    
    public bool IsFilterFor(IPublishedContent content) {
        var platformsPage = _contentCache.Special(Page);

        return content.Id == platformsPage?.Id;
    }

    public bool IsVisible(IPublishedContent content) {
        return _platformsPageAccessor.Value.GetAsync().GetAwaiter().GetResult()?.Page?.Kind == PublishedFileKind;
    }
    
    protected abstract SpecialContent Page { get; }
    protected abstract PublishedFileKind PublishedFileKind { get; }
}
